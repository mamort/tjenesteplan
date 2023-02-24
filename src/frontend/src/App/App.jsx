import React from 'react';
import { Router, Route, Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { history } from '../_helpers';
import { alertActions } from '../_actions';
import { userActions } from '../_actions';
import { PrivateRoute } from '../_components';
import { authenticationService } from '../_services/authenticationService';
import NavigationMenu from '../_components/NavigationMenu.jsx'
import { HomePage } from '../HomePage';
import { Vaktbytter, Vaktbytte, AcceptVaktbytter, AcceptVaktbytte } from '../Vaktbytte';
import { AcceptVakansvakter, AcceptVakansvakt, AvailableVakansvakter, AvailableVakansvakt } from '../Vakansvakt';
import { MinTjenesteplan, MineTjenesteplaner } from '../MinTjenesteplan';
import { Vakansvakter, Vakansvakt } from '../MinTjenesteplan/vakansvakt';
import { LoginPage, ResetPasswordPage, NewPasswordPage } from '../LoginPage';
import { InviteUser } from '../InviteUser';
import { RegisterPage } from '../RegisterPage';
import { RegisterInvitedUser } from '../RegisterPage/RegisterInvitedUser';
import { Tjenesteplaner } from '../Tjenesteplaner';
import { ListeFørerWeeklyTjenesteplan } from '../Listefører/WeeklyTjenesteplan';
import { ListeFørerTjenesteplanChanges } from '../Listefører/TjenesteplanChanges';
import { LegeProfile, Leger, AddLegeToAvdeling } from '../Leger';

import {
    CreateOrEditSykehus,
    CreateOrEditAvdeling,
    AssignListeforer
} from '../Administrator';

import {
    SykehusList,
    Sykehus,
    Avdeling
} from '../Sykehus';

import {
    CreateTjenesteplanConfigForm,
    CreateTjenesteplanWeeksForm,
    TjenesteplanLeger
} from '../Tjenesteplan'

function Alert(props) {
    const { alert, onClose } = props;

    if(alert.message) {
        return (<div className={`alert ${alert.type} alert-display`}>
            <span>{alert.message}</span>
            <i className="dripicons-cross alert-close" onClick={onClose} />
        </div>);
    }

    return (<div className={`alert ${alert.type} alert-hide`}></div>);
}

function LoginHeader(props) {
    const { user, handleLogout, isLoggedIn } = props;
    if(!user || !isLoggedIn) {
        return (<div/>);
    }
    return (
        <NavigationMenu user={user} handleLogout={handleLogout}/>
    );
}

class App extends React.Component {
    constructor(props) {
        super(props);
        this.handleLogout = this.handleLogout.bind(this);
        this.handleCloseAlert = this.handleCloseAlert.bind(this);

        const { dispatch } = this.props;
        history.listen((location, action) => {
            // clear alert on location change
            dispatch(alertActions.clear());
        });
    }

    componentDidMount() {
        if(authenticationService.isSessionTokenExpired()) {
            this.props.dispatch(userActions.logout());
        }
    }

    handleLogout() {
        this.props.dispatch(userActions.logout());
    }

    handleCloseAlert() {
        this.props.dispatch(alertActions.clear());
    }

    render() {
        const { alert, user, isLoggedIn } = this.props;
        return (
            <div>
                <Alert alert={alert} onClose={this.handleCloseAlert} />
                <Router history={history}>
                    <div>
                        <LoginHeader user={user} handleLogout={this.handleLogout} isLoggedIn={isLoggedIn} />
                        <div className="body">
                            <PrivateRoute exact path="/" component={HomePage} />
                            <Route path="/login" component={LoginPage} />
                            <Route exact path="/tilbakestill-passord/:id" component={NewPasswordPage} />
                            <Route exact path="/tilbakestill-passord" component={ResetPasswordPage} />
                            <Route path="/register/:id" component={RegisterInvitedUser} />
                            <PrivateRoute exact path="/register" component={RegisterPage} />
                            <PrivateRoute exact path="/inviter" component={InviteUser} />
                            <PrivateRoute exact path="/opprett-sykehus" component={CreateOrEditSykehus} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/opprett-avdeling" component={CreateOrEditAvdeling} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:id(\d+)/rediger" component={CreateOrEditAvdeling} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:id(\d+)" component={Avdeling} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:id(\d+)/tildel-listeforer" component={AssignListeforer} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/opprett-tjenesteplan" component={CreateTjenesteplanConfigForm} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/opprett-tjenesteplan/ukeplaner" component={CreateTjenesteplanWeeksForm} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/tjenesteplaner/:id(\d+)" component={CreateTjenesteplanWeeksForm} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/tjenesteplaner" component={Tjenesteplaner} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/leger/legg-til-lege" component={AddLegeToAvdeling} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/leger" component={Leger} />
                            <PrivateRoute exact path="/sykehus" component={SykehusList} />
                            <PrivateRoute exact path="/sykehus/:id(\d+)" component={Sykehus} />
                            <PrivateRoute exact path="/sykehus/:id(\d+)/rediger" component={CreateOrEditSykehus} />
                            <PrivateRoute exact path="/minetjenesteplaner/:id(\d+)" component={MinTjenesteplan} />
                            <PrivateRoute exact path="/minetjenesteplaner" component={MineTjenesteplaner} />
                            <PrivateRoute exact path="/minetjenesteplaner/:tjenesteplanId(\d+)/mine-vakansvaktforespørsler" component={Vakansvakter} />
                            <PrivateRoute exact path="/mine-vakansvaktforespørsler/:id(\d+)" component={Vakansvakt} />
                            <PrivateRoute exact path="/vakansvakter/:id(\d+)/aksepter" component={AcceptVakansvakt} />
                            <PrivateRoute exact path="/vakansvakter/:id(\d+)" component={AvailableVakansvakt} />
                            <PrivateRoute exact path="/minetjenesteplaner/:tjenesteplanId(\d+)/vakansvakter" component={AvailableVakansvakter} />
                            <PrivateRoute exact path="/minetjenesteplaner/:tjenesteplanId(\d+)/vaktbytter/aksepter" component={AcceptVaktbytter} />
                            <PrivateRoute exact path="/minetjenesteplaner/:tjenesteplanId(\d+)/vaktbytter/:id(\d+)/aksepter" component={AcceptVaktbytte} />
                            <PrivateRoute exact path="/minetjenesteplaner/:tjenesteplanId(\d+)/vaktbytter" component={Vaktbytter} />
                            <PrivateRoute exact path="/minetjenesteplaner/:tjenesteplanId(\d+)/vaktbytter/:id(\d+)" component={Vaktbytte} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/tjenesteplaner/:tjenesteplanId(\d+)/vakansvakter/aksepter" component={AcceptVakansvakter} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/tjenesteplaner/:tjenesteplanId(\d+)/vakansvakter" component={AvailableVakansvakter} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/tjenesteplaner/:tjenesteplanId(\d+)/ukentlig" component={ListeFørerWeeklyTjenesteplan} />
                            <PrivateRoute exact path="/sykehus/:sykehusId(\d+)/avdelinger/:avdelingId(\d+)/tjenesteplaner/:tjenesteplanId(\d+)/endringer" component={ListeFørerTjenesteplanChanges} />
                            <PrivateRoute exact path="/leger/:userId" component={LegeProfile} />
                        </div>
                    </div>
                </Router>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { alert } = state;
    return {
        alert,
        user: state.authentication.user,
        isLoggedIn: state.authentication.loggedIn
    };
}

const connectedApp = connect(mapStateToProps)(App);
export { connectedApp as App };