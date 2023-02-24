import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vakansvakt.actions';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { vakansvaktRequestStatusConstants } from '../_constants/vakansvaktRequestStatus.constants';

function VakansvaktStatus(props) {
    const { request, acceptVakansvakt } = props;

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
            <button
                type="button"
                className="btn btn-primary"
                onClick={acceptVakansvakt}
            >
                Ta vakt
            </button>
        </div>
    )
}

class AvailableVakansvakt extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const vakansvaktRequestId = this.props.match.params.id;

        if(vakansvaktRequestId) {
            this.props.initialize(parseInt(vakansvaktRequestId, 10));
        }
    }

    acceptVakansvakt(vakansvaktRequestId, date) {
        this.props.acceptVakansvakt(vakansvaktRequestId, date);
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
                        <h5>Vakansvakt</h5>

                        <table className="table">
                            <thead>
                                <tr>
                                    <th>Lege</th>
                                    <th>Dato</th>
                                    <th>Vakt</th>
                                    <th>Årsak</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{req.requestedBy}</td>
                                    <td>{moment.utc(req.date).format("dddd, Do MMMM")}</td>
                                    <td>{req.currentDagsplan.name}</td>
                                    <td>{req.requestedDagsplan.name}</td>
                                    <td>
                                        <VakansvaktStatus
                                            request={req}
                                            acceptVakansvakt={() => this.acceptVakansvakt(req.id, moment.utc(req.date))}
                                        />
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
            dispatch(actions.initializeAvailableVakansvakt(vakansvaktRequestId))
        },

        acceptVakansvakt: (vakansvaktRequestId, date) => {
            dispatch(actions.acceptVakansvakt(vakansvaktRequestId, date))
                .then(() => {
                    dispatch(actions.initializeAvailableVakansvakt(vakansvaktRequestId));
                });
        }
    }
  }

const AvailableVakansvaktConnected = connect(mapStateToProps, mapDispatchToProps)(AvailableVakansvakt);
export { AvailableVakansvaktConnected as AvailableVakansvakt };