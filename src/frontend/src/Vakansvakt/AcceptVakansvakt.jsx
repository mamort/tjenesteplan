import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vakansvakt.actions';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { vakansvaktRequestStatusConstants } from '../_constants/vakansvaktRequestStatus.constants';
import { breadcrumbActions } from '../_actions';

function VakansvaktStatus(props) {
    const { request, approveVakansvakt, rejectVakansvakt } = props;

    if(request.status === vakansvaktRequestStatusConstants.Approved){
        return (
            <span className="vaktbytter-text">
                Godkjent
            </span>
        );
    }

    if(request.status === vakansvaktRequestStatusConstants.Rejected){
        return (
            <span className="vaktbytter-text">
                Avvist av listefører
            </span>
        );
    }

    if(request.status === vakansvaktRequestStatusConstants.Accepted){
        return (
            <span className="vaktbytter-text">
                Tatt av {request.acceptedBy}
            </span>
        );
    }

    return (
        <div>
            <button type="button" className="btn btn-primary" onClick={approveVakansvakt}>
                Godkjenn vakansvakt
            </button>

            <button type="button" className="btn btn-secondary" onClick={rejectVakansvakt}>
                Avvis
            </button>
        </div>
    )
}

class AcceptVakansvakt extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const vakansvaktRequestId = this.props.match.params.id;

        if(vakansvaktRequestId) {
            this.props.initialize(parseInt(vakansvaktRequestId, 10))
                .then(() => {
                    const avdelingId = parseInt(this.props.request.avdelingId, 10);
                    const sykehus = this.props.sykehus.filter(s => s.avdelinger.filter(a => a.id == avdelingId).length > 0);
                    const avdeling = sykehus[0].avdelinger.find(a => a.id == avdelingId);
                    this.props.initBreadcrumbs(sykehus[0], avdeling);
                });
        }
    }

    approveVakansvakt(vakansvaktRequestId) {
        this.props.approveVakansvaktRequest(vakansvaktRequestId);
    }

    rejectVakansvakt(vakansvaktRequestId) {
        this.props.rejectVakansvaktRequest(vakansvaktRequestId);
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        if(this.props.isError && this.props.error.httpStatusCode === 404) {
            return (<Centered><h2>Forespørselen ble ikke funnet.</h2></Centered>);
        }

        const req = this.props.request;

        if(!req) {
            return <div/>;
        }

        return (
            <div className="vakansvakt">
                <Centered>
                    <div className="textbox">
                        <h5>Forespørsel om vakansvakt</h5>

                        <div className="table-scroll">
                        <table className="table">
                            <thead>
                                <tr>
                                    <th className="sticky-column">Lege</th>
                                    <th>Dato</th>
                                    <th>Vakt</th>
                                    <th>Årsak</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td className="sticky-column">{req.requestedBy}</td>
                                    <td>{moment.utc(req.date).format("dddd, Do MMMM")}</td>
                                    <td>{req.currentDagsplan.name}</td>
                                    <td>{req.requestedDagsplan.name}</td>
                                    <td>
                                        <VakansvaktStatus
                                            request={req}
                                            approveVakansvakt={() => this.approveVakansvakt(req.id)}
                                            rejectVakansvakt={() => this.rejectVakansvakt(req.id)}
                                        />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        </div>
                        <table className="table">
                            <tbody>
                                <tr>
                                    <td>
                                        <h6>Melding</h6>
                                        <div>{req.reason}</div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </Centered>
            </div>
        );
    }
}

/* The shared component "CenteredComponent" unfortunately breaks the responsive table scrolling
   so this is used to center the content instead, which is not as good but works in this case
*/
class Centered extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="absolute-position fill-height fill-width">
                <div className="container fill-height fill-width">
                    <div className="row justify-content-center fill-height fill-width">
                        {this.props.children}
                    </div>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        dagsplaner: state.dagsplaner.alleDagsplaner,
        sykehus: state.sykehus.sykehus,
        request: state.vakansvaktRequests.request,
        isError: state.vakansvaktRequests.isError,
        error: state.vakansvaktRequests.error,
        isFetching: state.vakansvaktRequests.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (vakansvaktRequestId) => {
            return dispatch(actions.initializeAcceptVakansvakt(vakansvaktRequestId))
        },


        initBreadcrumbs: (sykehus, avdeling) => {
            dispatch(
                breadcrumbActions.set(
                  [
                    { name: "Sykehus", link: "/sykehus"},
                    { name: sykehus.name, link: "/sykehus/" + sykehus.id },
                    { name: avdeling.name, link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id }
                  ]
                )
            )
        },

        approveVakansvaktRequest: (vakansvaktRequestId) => {
            dispatch(actions.approveVakansvaktRequest(vakansvaktRequestId))
        },

        rejectVakansvaktRequest: (vakansvaktRequestId) => {
            dispatch(actions.rejectVakansvaktRequest(vakansvaktRequestId))
        }
    }
  }

const AcceptVakansvaktConnected = connect(mapStateToProps, mapDispatchToProps)(AcceptVakansvakt);
export { AcceptVakansvaktConnected as AcceptVakansvakt };