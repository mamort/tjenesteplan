import React from 'react';
import { connect } from 'react-redux';
import { userActions } from '../_actions';

class ResetPasswordPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            email: '',
            submitted: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleResetOk = this.handleResetOk.bind(this);

        this.props.dispatch(userActions.resetPasswordReset());
    }

    handleChange(e) {
        const { name, value } = e.target;
        this.setState({ [name]: value });
    }

    handleSubmit(e) {
        e.preventDefault();

        this.setState({ submitted: true });
        const { email } = this.state;
        const { dispatch } = this.props;
        if (email) {
            dispatch(userActions.resetPassword(email));
        }
    }

    handleResetOk(e) {
        e.preventDefault();
        this.props.dispatch(userActions.resetPasswordReset());
        this.setState({
            email: '',
            submitted: false
        });
    }

    render() {
        const { email, submitted } = this.state;
        const { resettingPassword, isReset } = this.props;

        return (
            <div className="absolute-position fill-height fill-width">
                <div className="container fill-height fill-width">
                    <div className="row align-items-center justify-content-center fill-height fill-width">
                        <div className="col-lg-6 col-md-8 col-sm-8 col-xs-8 login-col">
                            <ResetPasswordContent
                                email={email}
                                submitted={submitted}
                                isReset={isReset}
                                isResetting={resettingPassword}
                                handleChange={this.handleChange}
                                handleSubmit={this.handleSubmit}
                                handleResetOk={this.handleResetOk}
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

function ResetPasswordContent(props) {

    const {
        email,
        isReset,
        submitted,
        isResetting,
        handleSubmit,
        handleChange,
        handleResetOk
     } = props;

    if(isReset) {
        return (
            <div>
                En e-post er sendt til {email} med en lenke for å tilbakestille passordet for denne kontoen.
                <br/><br/>
                <button className="btn btn-primary" onClick={handleResetOk}>Ok</button>
            </div>
        );
    }

    return (
        <div>
            <h2>Tilbakestill passord</h2>
            <form name="form" onSubmit={handleSubmit}>
                <div className={'form-group' + (submitted && !email ? ' has-error' : '')}>
                    <label htmlFor="email">E-post</label>
                    <input type="text" className="form-control" name="email" value={email} onChange={handleChange} />
                    {submitted && !email &&
                        <div className="help-block">e-post må fylles ut</div>
                    }
                </div>
                <div className="form-group">
                    <button className="btn btn-primary">Tilbakestill passord</button>
                    {isResetting &&
                        <div className="loader-small" />
                    }
                </div>
            </form>
        </div>
    )
}

function mapStateToProps(state) {
    const { resettingPassword, isReset } = state.authentication;
    return {
        resettingPassword,
        isReset
    };
}

const resetPage = connect(mapStateToProps)(ResetPasswordPage);
export { resetPage as ResetPasswordPage };