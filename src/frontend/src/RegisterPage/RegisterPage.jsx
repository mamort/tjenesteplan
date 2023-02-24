import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';

import { userActions } from '../_actions';
import { roleConstants } from '../_constants'
import * as actions from './register.actions';

import { NotFoundPage } from '../_components/NotFoundPage.jsx';
import { Loader } from '../_components/Loader.jsx';

function RoleInput(props) {
    const user = props.user;
    const validRoles = props.validRoles;

    return(
        <div className={'form-group' + (props.submitted && !user.role ? ' has-error' : '')}>
            <label htmlFor="role">Type bruker</label>
            <select  className="form-control" name="role" value={user.role} onChange={props.handleChange}>
                {validRoles.map((role, index) =>
                    (<option value={role.value} key={index}>{role.name}</option>)
                )}
            </select>

            {props.submitted && !user.role &&
                <div className="help-block">Type bruker må angis</div>
            }
        </div>
    )
}

class RegisterPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: {
                firstname: '',
                lastname: '',
                username: '',
                password: '',
                role: 1
            },
            submitted: false
        };

        this.getValidRoles = this.getValidRoles.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        const id = this.props.match.params.id;
        if(id) {
            this.props.getInvitation(id);
            this.setState({ id: id.toLowerCase() });
        }
      }

    getValidRoles() {
        const user = this.props.loggedInUser;

        if(!user) {
            return [ { name: 'Lege', value: roleConstants.Lege } ];
        }

        if(user.role === roleConstants.Overlege) {
            return [ { name: 'Lege', value: roleConstants.Lege } ];
        }

        if(user.role === roleConstants.Admin) {
            return [ { name: 'Lege', value: roleConstants.Lege },
                     { name: 'Overlege', value: roleConstants.Overlege } ];
        }

        return [];
    }

    handleChange(event) {
        const { name, value } = event.target;
        const { user } = this.state;
        this.setState({
            user: {
                ...user,
                [name]: value
            }
        });
    }

    handleSubmit(event) {
        event.preventDefault();

        this.setState({ submitted: true });
        const { user } = this.state;
        if (user.firstname && user.lastname && user.username && user.password && user.role) {
            this.props.registerUser(user);
        }
    }

    render() {
        const { registering, invitations  } = this.props;
        const { user, submitted } = this.state;

        const id = this.state.id;
        let email = user.username;
        if(id) {
            const invitation = invitations.invitations[id];
            if(invitation) {
                email = invitation.email;
            }
        }

        if(invitations.isFetching) {
            return (<Loader />);
        }

        if(invitations.isFailed && invitations.error.httpStatusCode === 404) {
            return (
                <NotFoundPage message="Beklager, denne invitasjonen finnes ikke" />
            )
        }

        return (
            <div className="absolute-position fill-height fill-width">
                <div className="container fill-height fill-width">
                    <div className="row align-items-center justify-content-center fill-height fill-width">
                        <div className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                            <h2>Registrer ny bruker</h2>
                            <form name="form" onSubmit={this.handleSubmit}>
                                <div className={'form-group' + (submitted && !user.firstname ? ' has-error' : '')}>
                                    <label htmlFor="firstname">Fornavn</label>
                                    <input type="text" className="form-control" name="firstname" value={user.firstname} onChange={this.handleChange} />
                                    {submitted && !user.firstname &&
                                        <div className="help-block">Fornavn er obligatorisk</div>
                                    }
                                </div>
                                <div className={'form-group' + (submitted && !user.lastname ? ' has-error' : '')}>
                                    <label htmlFor="lastname">Etternavn</label>
                                    <input type="text" className="form-control" name="lastname" value={user.lastname} onChange={this.handleChange} />
                                    {submitted && !user.lastname &&
                                        <div className="help-block">Etternavn er obligatorisk</div>
                                    }
                                </div>
                                <div className={'form-group' + (submitted && !email ? ' has-error' : '')}>
                                    <label htmlFor="username">E-post</label>
                                    <input type="text" className="form-control" name="username" value={email} onChange={this.handleChange} />
                                    {submitted && !user.username &&
                                        <div className="help-block">E-post er obligatorisk</div>
                                    }
                                </div>
                                <div className={'form-group' + (submitted && !user.password ? ' has-error' : '')}>
                                    <label htmlFor="password">Passord</label>
                                    <input type="password" className="form-control" name="password" value={user.password} onChange={this.handleChange} />
                                    {submitted && !user.password &&
                                        <div className="help-block">Passord er obligatorisk</div>
                                    }
                                </div>
                                <RoleInput
                                    user={user}
                                    validRoles={this.getValidRoles()}
                                    handleChange={this.handleChange}
                                    submitted={submitted}
                                />
                                <div className="form-group">
                                    <button className="btn btn-primary">Register</button>
                                    {registering &&
                                        <img src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                                    }
                                    <Link to="/login" className="btn btn-link">Cancel</Link>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        registering: state.registration.registering,
        loggedInUser: state.authentication.user,
        invitations: state.invitations
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      getInvitation: (id) => {
        dispatch(actions.getInvitation(id));
      },

      registerUser: (user) => {
        dispatch(userActions.register(user));
      }
    }
  }

const connectedRegisterPage = connect(mapStateToProps, mapDispatchToProps)(RegisterPage);
export { connectedRegisterPage as RegisterPage };