import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './vaktbytte.actions';
import { Loader } from '../_components/Loader.jsx';
import { vaktChangeRequestStatusConstants } from '../_constants/vaktChangeRequestStatus.constants';
import { vaktChangeRequestReplyStatusConstants } from '../_constants/vaktChangeRequestReplyStatus.constants';
import { breadcrumbActions } from '../_actions';

function VaktbytteButton(props) {
    const { tjenesteplanId, request, hasVaktbytteDateSuggestions } = props;

    if(request.status === vaktChangeRequestStatusConstants.Completed){
        return (
            <span className="vaktbytter-text">
                Du har byttet vakten din {moment.utc(request.date).format("dddd, Do MMMM")}
                &nbsp;med {request.fullname}. Din nye vakt er {moment.utc(request.chosenChangeDate).format("dddd, Do MMMM")}.
            </span>
        );
    }

    if(!hasVaktbytteDateSuggestions) {
        return (
            <span className="vaktbytter-text">
                Ingen bytteforslag er mottatt.
            </span>
        );
    }

    return (
        <Link to={"/minetjenesteplaner/" + tjenesteplanId + "/vaktbytter/"+request.id+"/aksepter"}>
            <button type="button" className="btn btn-primary">
                Aksepter vaktbytte
            </button>
        </Link>
    )
}

class AcceptVaktbytter extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            requests: []
        };
    }

    componentDidMount() {
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
        this.props.initialize(tjenesteplanId)
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == this.props.sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == this.props.avdelingId);
                this.props.initBreadcrumbs(sykehus, avdeling);

                let requests = this.props.requests
                    .map(r => {
                        let fullname = "";
                        const reply = r.replies.find(r => r.status === vaktChangeRequestReplyStatusConstants.Accepted);
                        if(reply){
                            fullname = reply.fullname;
                        }
                        return {
                            date: r.date,
                            status: r.status,
                            fullname: fullname,
                            chosenChangeDate: r.chosenChangeDate,
                            replies: r.replies
                        };
                    });

                requests = requests.concat(this.props.receivedRequests
                    .filter(r => r.status === vaktChangeRequestStatusConstants.Completed && r.reply.status === vaktChangeRequestReplyStatusConstants.Accepted)
                    .map(r => {
                        return {
                            date: r.chosenChangeDate,
                            status: r.status,
                            fullname: r.requestedBy,
                            chosenChangeDate: r.date,
                            replies: [r.reply]
                        };
                    })
                );

                requests.sort((r1, r2) => moment.utc(r2.date) - moment.utc(r1.date));

                this.setState({
                    requests
                });
            });
    }

    hasVaktbytteDateSuggestions(request) {
        let count = 0;
        for(let i = 0; i < request.replies.length; i++) {
            const reply = request.replies[i];
            count += reply.alternatives.length;
        }

        return count > 0;
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        const tjenesteplanId = this.props.match.params.tjenesteplanId;
        const requests = this.state.requests;

        return (
            <div className="container vaktbytter">
                <h1>Mine vaktbytter</h1>
                {requests.map((req, index) => (
                    <div className="textbox" key={index}>
                        <strong>{moment.utc(req.date).format("dddd, Do MMMM YYYY")}</strong>
                        <VaktbytteButton
                            tjenesteplanId={tjenesteplanId}
                            request={req}
                            hasVaktbytteDateSuggestions={this.hasVaktbytteDateSuggestions(req)}
                        />
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
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        requests: state.vaktChangeRequests.requests,
        receivedRequests: state.vaktChangeRequests.received.requests,
        isFetching: state.vaktChangeRequests.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        initialize: (tjenesteplanId) => {
            return dispatch(actions.initializeAcceptVaktbytter(tjenesteplanId))
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

const AcceptVaktbytterConnected = connect(mapStateToProps, mapDispatchToProps)(AcceptVaktbytter);
export { AcceptVaktbytterConnected as AcceptVaktbytter };