import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vaktbytte.actions';
import { vaktChangeRequestStatusConstants } from '../_constants/vaktChangeRequestStatus.constants';
import { Loader } from '../_components/Loader.jsx';
import { breadcrumbActions } from '../_actions';

class Vaktbytter extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId);
        this.props.initialize(tjenesteplanId)
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == this.props.sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == this.props.avdelingId);
                this.props.initBreadcrumbs(
                    sykehus,
                    avdeling,
                    {
                        id: tjenesteplanId,
                        name: this.props.tjenesteplanName
                    }
                );
            });
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        const tjenesteplanId = this.props.match.params.tjenesteplanId;
        const requests = this.props.receivedRequests;

        return (
            <div className="container vaktbytter">
                <h1>Svar på forespørsler om vaktbytte</h1>
                {requests.map((req, index) => (
                    <div className="textbox">
                        <Vaktbytte tjenesteplanId={tjenesteplanId} request={req} />
                    </div>
                ))}
            </div>
        );
    }
}

function ChangeDate(props) {
    const { date } = props
    return (
        <strong>{moment.utc(date).format("dddd, Do MMMM")}</strong>
    )
}

function Vaktbytte(props) {
    const { tjenesteplanId, request } = props;

    if(request.status === vaktChangeRequestStatusConstants.Completed) {
        return (
            <div>Vaktbytte forespurt av {request.requestedBy} på <ChangeDate date={request.date} /> er fullført.</div>
        )
    }

    if(request.status === vaktChangeRequestStatusConstants.Canceled) {
         return (
             <div>Vaktbytte forespurt av {request.requestedBy} på <ChangeDate date={request.date} /> er kansellert.</div>
         )
     }

    return (
        <div>
            {request.requestedBy} ønsker å bytte vakten sin&nbsp;
            <ChangeDate date={request.date} />.
            <Link to={"/minetjenesteplaner/" + tjenesteplanId + "/vaktbytter/"+request.vaktChangeRequestId}>
                <button type="button" className="btn btn-primary">
                    Svar på forespørsel
                </button>
            </Link>
        </div>
    )
}

function mapStateToProps(state) {
    return {
        dagsplaner: state.dagsplaner.alleDagsplaner,
        sykehus: state.sykehus.sykehus,
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        tjenesteplanName: state.mintjenesteplan.name,
        receivedRequests: state.vaktChangeRequests.received.requests,
        isFetching: state.vaktChangeRequests.isFetching ||
                    state.vaktChangeRequests.received.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (tjenesteplanId) => {
            return dispatch(actions.initializeVaktbytter(tjenesteplanId))
        },

        initBreadcrumbs: (sykehus, avdeling, tjenesteplan) => {
            dispatch(
                breadcrumbActions.set(
                    [{ name: sykehus.name },
                     { name: avdeling.name },
                     { name: tjenesteplan.name, link: '/minetjenesteplaner/' + tjenesteplan.id }
                    ]
                )
            );
        }
    }
  }

const VaktbytterConnected = connect(mapStateToProps, mapDispatchToProps)(Vaktbytter);
export { VaktbytterConnected as Vaktbytter };