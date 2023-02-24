import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import queryString from 'query-string-es5';
import { userActions } from '../_actions';

class NewPasswordPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            password: '',
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

        this.props.dispatch(userActions.resetNewPassword());
    }

    handleChange(e) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ submitted: true });
        const { password } = this.state;
        const { dispatch } = this.props;
        if (password && password.length > 6) {
            const token = this.props.match.params.id;
            dispatch(userActions.saveNewPassword(token, password));
        }
    }

    render() {
        const { email, submitted } = this.state;
        const { isSaving, isSaved } = this.props;

        return (
            <div className="absolute-position fill-height fill-width">
                <div className="container fill-height fill-width">
                    <div className="row align-items-center justify-content-center fill-height fill-width">
                        <div className="col-lg-6 col-md-8 col-sm-8 col-xs-8 login-col">
                            <NewPasswordContent
                                password={email}
                                submitted={submitted}
                                isSaving={isSaving}
                                isSaved={isSaved}
                                handleChange={this.handleChange}
                                handleSubmit={this.handleSubmit}
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

function NewPasswordContent(props) {

    const {
        password,
        isSaved,
        submitted,
        isSaving,
        handleSubmit,
        handleChange
     } = props;

    if(isSaved) {
        return (
            <div>
                Nytt passord er lagret. Du kan nå <Link to="/login">logge inn</Link> på nytt.
            </div>
        );
    }

    return (
        <div>
            <h2>Sett nytt passord</h2>
            <form name="form" onSubmit={handleSubmit}>
                <div className={'form-group' + (submitted && !password ? ' has-error' : '')}>
                    <label htmlFor="password">Nytt passord</label>
                    <input type="text" className="form-control" name="password" value={password} onChange={handleChange} />
                    {submitted && (!password || password.length < 6) &&
                        <div className="help-block">Feltet må fylles ut og passordet må bestå av flere enn 6 tegn</div>
                    }
                </div>
                <div className="form-group">
                    <button className="btn btn-primary">Lagre nytt passord</button>
                    {isSaving &&
                        <div className="loader-small" />
                    }
                </div>
            </form>
        </div>
    )
}

function mapStateToProps(state) {
    return {
        isSaving: state.authentication.isSavingNewPassword,
        isSaved: state.authentication.isNewPasswordSaved
    };
}

const newPasswordPage = connect(mapStateToProps)(NewPasswordPage);
export { newPasswordPage as NewPasswordPage };