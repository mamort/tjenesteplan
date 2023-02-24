import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vakansvakt.actions';
import { Loader } from '../../_components/Loader.jsx';
import { vakansvaktRequestStatusConstants } from '../../_constants/vakansvaktRequestStatus.constants';
import { breadcrumbActions } from '../../_actions';

function VakansvaktStatus(props) {
    const { request } = props;

    if(request.status === vakansvaktRequestStatusConstants.Approved){
        return (
            <span className="vaktbytter-text">
                Vakansvakt-forespørsel er godkjent av listefører.
            </span>
        );
    }

    if(request.status === vakansvaktRequestStatusConstants.Rejected){
        return (
            <span className="vaktbytter-text">
                Vakansvakt-forespørsel er avvist av listefører.
            </span>
        );
    }

    if(request.status === vakansvaktRequestStatusConstants.Accepted){
        return (
            <span className="vaktbytter-text">
                Vakansvakt-forespørsel er godkjent og overtatt av lege.
            </span>
        );
    }

    return (
        <span className="vaktbytter-text">
            Venter på godkjenning.
        </span>
    )
}

class Vakansvakter extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
        this.props.initialize(tjenesteplanId)
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == this.props.sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == this.props.avdelingId);
                this.props.initBreadcrumbs(sykehus, avdeling);
            });
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        const requests = this.props.requests;

        return (
            <div className="container vakansvakter">
                <h1>Vakansvakt forespørsler</h1>
                {requests.map((req, index) => (
                    <div className="textbox" key={index}>
                        <strong>{moment.utc(req.date).format("dddd, Do MMMM")}</strong>
                        <VakansvaktStatus request={req} />
                    </div>
                ))}
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        sykehus: state.sykehus.sykehus,
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        requests: state.vakansvaktRequests.requests,
        isFetching: state.vakansvaktRequests.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (tjenesteplanId) => {
            return dispatch(actions.initialize(tjenesteplanId))
        },

        initBreadcrumbs: (sykehus, avdeling) => {
            dispatch(
                breadcrumbActions.set(
                    [{ name: sykehus.name },
                     { name: avdeling.name }
                    ]
                )
            );
        }
    }
  }

const VakansvakterConnected = connect(mapStateToProps, mapDispatchToProps)(Vakansvakter);
export { VakansvakterConnected as Vakansvakter };