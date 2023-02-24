import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vakansvakt.actions';
import { Loader } from '../../_components/Loader.jsx';
import { CenteredComponent } from '../../_components/CenteredComponent.jsx';
import { vakansvaktRequestStatusConstants } from '../../_constants/vakansvaktRequestStatus.constants';

function VakansvaktStatus(props) {
    const { request } = props;

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

    if(request.status === vakansvaktRequestStatusConstants.Rejected){
        return (
            <span className="vaktbytter-text">
                Vakansvakt-forespørsel er godkjent og tatt av {request.acceptedBy}.
            </span>
        );
    }

    return (
        <span className="vaktbytter-text">
            Venter på godkjenning
        </span>
    )
}

class Vakansvakt extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const vakansvaktRequestId = parseInt(this.props.match.params.id, 10);

        if(vakansvaktRequestId) {
            this.props.initialize(vakansvaktRequestId);
        }
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        if(this.props.isError && this.props.error.httpStatusCode === 404) {
            return (<CenteredComponent><h2>Forespørselen ble ikke funnet.</h2></CenteredComponent>);
        }

        const req = this.props.request;

        if(!req) {
            return <div/>;
        }

        return (
            <div className="vakansvakt">
                <CenteredComponent>
                    <div className="textbox">
                        <h5>Forespørsel om vakansvakt</h5>

                        <table className="table">
                            <thead>
                                <tr>
                                    <th>Lege</th>
                                    <th>Dato</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{req.requestedBy}</td>
                                    <td>{moment.utc(req.date).format("dddd, Do MMMM")}</td>
                                    <td>
                                        <VakansvaktStatus request={req} />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <h6>Årsak</h6>
                                        <div>{req.reason}</div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </CenteredComponent>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
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
            dispatch(actions.initializeAcceptVakansvakt(vakansvaktRequestId))
        }
    }
  }

const VakansvaktConnected = connect(mapStateToProps, mapDispatchToProps)(Vakansvakt);
export { VakansvaktConnected as Vakansvakt };