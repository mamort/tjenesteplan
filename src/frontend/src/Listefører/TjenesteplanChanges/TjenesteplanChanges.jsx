import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import * as actions from './TjenesteplanChanges.actions';
import { Loader } from '../../_components/Loader.jsx';
import { breadcrumbActions } from '../../_actions';

class TjenesteplanChanges extends React.Component {

    constructor(props) {
        super(props);

        this.handleUndoChange = this.handleUndoChange.bind(this);
    }

    componentDidMount() {
        const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
        const avdelingId = parseInt(this.props.match.params.avdelingId, 10);
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
        this.props.initialize(tjenesteplanId)
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
                this.props.initBreadcrumbs(sykehus, avdeling);
            });
    }

    handleUndoChange(change) {
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);

        if(change.type === 'vakansvakt') {
            this.props.undoVakansvakt(tjenesteplanId, change.id);
        } else if(change.type === 'vaktchange') {
            this.props.undoVaktChange(tjenesteplanId, change.id);
        }
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        const allVaktChangeRequests = this.props.allVaktChangeRequests;
        const vakansvaktRequests = this.props.approvedVakansvaktRequests;

        let changes = allVaktChangeRequests.map(vc => {
            return {
                type: 'vaktchange',
                id: vc.id,
                date: vc.date,
                user: {
                    fullname: vc.fullname
                },
                vaktbytte: {
                    chosenLege: vc.chosenReply ? vc.chosenReply.fullname : null,
                    date: vc.chosenChangeDate
                }
            };
        });

        const vakansvaktChanges = vakansvaktRequests.map(v => {
            return {
                type: 'vakansvakt',
                id: v.id,
                date: v.date,
                user: {
                    fullname: v.requestedBy
                }
            }
        });

        changes = changes.concat(vakansvaktChanges)

        return (
            <div className="container tjenesteplanChanges">
                <h1>Tjenesteplan endringer</h1>
                {changes.map((change, index) => (
                    <div className="textbox" key={index}>
                        <Change change={change} handleUndo={this.handleUndoChange} />
                    </div>
                ))}
            </div>
        );
    }
}

function Vaktbytte(props) {
    const { change, handleUndo } = props;
    return (
        <div>
            <h5>Vaktbytte</h5>

            <div className="table-scroll">
            <table className="table">
                <thead>
                    <tr>
                        <th className="sticky-column">Forespurt av</th>
                        <th>Dato</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td className="sticky-column">{change.user.fullname}</td>
                        <td>{moment.utc(change.date).format("dddd, Do MMMM")}</td>
                        <td>
                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={() => handleUndo(change)}
                            >
                                Rull tilbake endring
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
            </div>
            { change.vaktbytte.chosenLege
                ?   <div>
                        Vakten ble byttet med {change.vaktbytte.chosenLege} mot vakten {moment.utc(change.vaktbytte.date).format("dddd, Do MMMM")}
                    </div>
                :   <div>
                        Vaktbytte er ikke fullført.
                    </div>
            }
        </div>
    )
}


function Vakansvakt(props) {
    const { change, handleUndo } = props;
    return (
        <div>
            <h5><Link to={"/vakansvakter/"+change.id+"/aksepter"}>Vakansvakt</Link></h5>

            <div className="table-scroll">
            <table className="table">
                <thead>
                    <tr>
                        <th className="sticky-column">Forespurt av</th>
                        <th>Dato</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td className="sticky-column">{change.user.fullname}</td>
                        <td>{moment.utc(change.date).format("dddd, Do MMMM")}</td>
                        <td>
                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={() => handleUndo(change)}
                            >
                                Rull tilbake endring
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
            </div>
        </div>
    )
}


function Change(props) {
    const { change, handleUndo } = props;

    if(change.type === 'vakansvakt') {
        return <Vakansvakt change={change} handleUndo={handleUndo} />
    }

    return <Vaktbytte change={change} handleUndo={handleUndo} />
}

function mapStateToProps(state) {
    return {
        sykehus: state.sykehus.sykehus,
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        tjenesteplanChanges: state.tjenesteplanChanges.changes,
        allVaktChangeRequests: state.vaktChangeRequests.allRequests,
        approvedVakansvaktRequests: state.vakansvaktRequests.approvedRequests,
        isFetching: state.vakansvaktRequests.isFetching ||
                    state.vaktChangeRequests.isFetching ||
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
                    [{ name: sykehus.name },
                     { name: avdeling.name }
                    ]
                )
            );
        },

        undoVakansvakt: (tjenesteplanId, vakansvaktRequestId) => {
            dispatch(actions.undoVakansvakt(tjenesteplanId, vakansvaktRequestId))
                .then(() => {
                    dispatch(actions.initialize(tjenesteplanId));
                });
        },

        undoVaktChange: (tjenesteplanId, vaktChangeRequestId) => {
            dispatch(actions.undoVaktChange(tjenesteplanId, vaktChangeRequestId))
                .then(() => {
                    dispatch(actions.initialize(tjenesteplanId));
                });
        }
    }
  }

  export default connect(mapStateToProps, mapDispatchToProps)(TjenesteplanChanges);