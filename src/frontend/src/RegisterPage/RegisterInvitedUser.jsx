import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';

import { userActions } from '../_actions';
import * as actions from './register.actions';
import Select from 'react-select';
import { NotFoundPage } from '../_components/NotFoundPage.jsx';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import legeSpesialiteterApi from '../Api/LegeSpesialiteter';

class RegisterInvitedUser extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            user: {
                username: '',
                firstname: '',
                lastname: '',
                password: '',
                legeSpesialitetId: null
            },
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        const id = this.props.match.params.id;
        if(id) {
            this.props.initialize(id)
                .then(() => {
                    const invitation = this.props.invitations.invitations[id];

                    this.setState({
                        id: id.toLowerCase(),
                        user: {
                            invitationId: id.toLowerCase(),
                            email: invitation.email,
                            firstname: invitation.firstname,
                            lastname: invitation.lastname,
                            legeSpesialitetId: invitation.legeSpesialitetId
                        }
                    });
                });
        }
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
        if (user.firstname && user.lastname && user.password) {
            this.props.registerUser(user);
        }
    }

    render() {
        const { registering, invitations, legeSpesialiteter  } = this.props;
        const { user, submitted } = this.state;

        const id = this.state.id;
        if(invitations.isFetching) {
            return (<Loader />);
        }

        if(!id || user.email === '' ||
            (invitations.isFailed && invitations.error.httpStatusCode === 404)) {
            return (
                <NotFoundPage message="Beklager, denne invitasjonen finnes ikke" />
            )
        }

        return (
            <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                <h2>Registrer deg</h2>
                <form name="form" onSubmit={this.handleSubmit}>

                    <InputField
                        label="Brukernavn"
                        name="username"
                        value={user.email}
                        submitted={submitted}
                        isDisabled={true}
                        onChange={() => {}}
                    />

                    <InputField
                        label="Fornavn"
                        name="firstname"
                        value={user.firstname}
                        submitted={submitted}
                        onChange={this.handleChange}
                    />

                    <InputField
                        label="Etternavn"
                        name="lastname"
                        value={user.lastname}
                        submitted={submitted}
                        onChange={this.handleChange}
                    />

                    <div className="form-group">
                        <label htmlFor="legeSpesialitet">Spesialitet</label>
                        <SelectInput
                            name="legeSpesialitetId"
                            label="Velg spesialitet"
                            selectedValue={user.legeSpesialitetId}
                            handleInvitationChange={this.handleChange}
                            options={legeSpesialiteter.map(s => ({ value: s.id, label: s.name }))}
                        />
                    </div>

                    <InputField
                        label="Passord"
                        name="password"
                        type="password"
                        value={user.password}
                        submitted={submitted}
                        onChange={this.handleChange}
                    />

                    <div className="form-group">
                        <button className="btn btn-primary">Register</button>
                        {registering &&
                            <img src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                        }
                        <Link to="/login" className="btn btn-link">Cancel</Link>
                    </div>
                </form>
            </CenteredComponent>
        );
    }
}

function InputField(props) {
    const { name, label, value, submitted, onChange} = props;

    const isDisabled = props.isDisabled || false;
    const type = props.type || "text";

    return (
        <div className={'form-group' + (submitted && !value ? ' has-error' : '')}>
            <label htmlFor={name}>{label}</label>
            <input
                type={type}
                className="form-control"
                name={name}
                value={value}
                onChange={onChange}
                disabled={isDisabled}
            />
            {submitted && !value &&
                <div className="help-block">{label} er obligatorisk.</div>
            }
        </div>
    )
}


function SelectInput(props) {
    const { name, options, label, selectedValue, handleInvitationChange } = props;

    let selectedOption = { value: -1, label: label };

    if(selectedValue){
      selectedOption = options.find(o => o.value == selectedValue);
    }else{
      options.push(selectedOption);
    }

    return (<Select
      name={name}
      defaultValue={selectedValue}
      value={selectedOption}
      options={options}
      onChange={event => handleInvitationChange({target: { name, value: event.value }})}
      hideSelectedOptions={true}
    />);
  }

function mapStateToProps(state) {
    return {
        registering: state.registration.registering,
        loggedInUser: state.authentication.user,
        invitations: state.invitations,
        legeSpesialiteter: state.legeSpesialiteter.spesialiteter,
        isFetching: state.invitations.isFetching || state.legeSpesialiteter.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (id) => {
            dispatch(legeSpesialiteterApi.actions.getLegeSpesialiteter());
            return dispatch(actions.getInvitation(id))
        },

        registerUser: (user) => {
            dispatch(userActions.register(user))
        }
    }
  }

const connectedRegisterPage = connect(mapStateToProps, mapDispatchToProps)(RegisterInvitedUser);
export { connectedRegisterPage as RegisterInvitedUser };