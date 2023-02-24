import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vakansvakt.actions';
import { Loader } from '../_components/Loader.jsx';
import { breadcrumbActions } from '../_actions';

function VakansvaktStatus(props) {
    const { request } = props;

    return (
        <Link to={"/vakansvakter/"+request.id+"/aksepter"}>
            <button type="button" className="btn btn-primary">
                Godkjenn vakansvakt
            </button>
        </Link>
    )
}

class AcceptVakansvakter extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
        const avdelingId = parseInt(this.props.match.params.avdelingId, 10);
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);

        if(tjenesteplanId) {
            this.props.initialize(tjenesteplanId)
                .then(() => {
                    const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
                    const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
                    this.props.initBreadcrumbs(sykehus, avdeling);
                });
        }
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
                <h1>Godkjenn vakansvakter</h1>
                {requests.map((req, index) => (
                    <div className="textbox" key={index}>
                        <span>{req.requestedBy} ønsker å sette vakten&nbsp;
                        <strong>{moment.utc(req.date).format("dddd, Do MMMM")}</strong> vakant.</span>
                        <VakansvaktStatus request={req} />
                    </div>
                ))}
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        dagsplaner: state.dagsplaner.alleDagsplaner,
        sykehus: state.sykehus.sykehus,
        requests: state.vakansvaktRequests.requests,
        isFetching: state.vakansvaktRequests.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (tjenesteplanId) => {
            return dispatch(actions.initialize(tjenesteplanId));
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
            );
        }
    }
  }

const AcceptVakansvakterConnected = connect(mapStateToProps, mapDispatchToProps)(AcceptVakansvakter);
export { AcceptVakansvakterConnected as AcceptVakansvakter };