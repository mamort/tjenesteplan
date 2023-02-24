import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vakansvakt.actions';
import { Loader } from '../_components/Loader.jsx';
import { breadcrumbActions } from '../_actions';

class AvailableVakansvakter extends React.Component {

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

    acceptVakansvakt(vakansvaktRequestId, date) {
        this.props.acceptVakansvakt(vakansvaktRequestId, date);
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
                <h1>Ledige vakansvakter</h1>
                {requests.map((req, index) => (
                    <div className="textbox" key={index}>
                        <strong>{moment.utc(req.date).format("dddd, Do MMMM")}</strong>
                        <button
                            type="button"
                            className="btn btn-primary"
                            onClick={() => this.acceptVakansvakt(req.id, moment.utc(req.date))}
                        >
                            Ta vakt
                        </button>
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
        requests: state.vakansvaktRequests.availableRequests,
        isFetching: state.vakansvaktRequests.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (tjenesteplanId) => {
            return dispatch(actions.initializeAvailableVakansvakter(tjenesteplanId));
        },


        initBreadcrumbs: (sykehus, avdeling) => {
            dispatch(
                breadcrumbActions.set(
                    [{ name: sykehus.name },
                     { name: avdeling.name }
                    ]
                )
            );
        },

        acceptVakansvakt: (vakansvaktRequestId, date) => {
            dispatch(actions.acceptVakansvakt(vakansvaktRequestId, date))
                .then(() => {
                    dispatch(actions.initializeAvailableVakansvakter());
                });
        }
    }
  }

const AvailableVakansvakterConnected = connect(mapStateToProps, mapDispatchToProps)(AvailableVakansvakter);
export { AvailableVakansvakterConnected as AvailableVakansvakter };