import React from 'react';
import { connect } from 'react-redux';
import { history } from '../_helpers';
import { withRouter } from "react-router";
import * as actions from './tjenesteplan.actions';
import { DayPicker } from '../_components/DayPicker';
import { breadcrumbActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';

class CreateTjenesteplanConfig extends React.Component {
    constructor(props) {
        super(props);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleDayChange = this.handleDayChange.bind(this);

        this.state = {
            name: "",
            startDate: undefined,
            numberOfLeger: 1
        };
    }

    componentDidMount() {
        const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
        const avdelingId = parseInt(this.props.match.params.avdelingId, 10);

        this.props.initialize()
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
                this.props.initBreadcrumbs(sykehus, avdeling);
            });
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
          [name]: value
        }, () => {
            this.props.tjenesteplanConfigChanged(this.state);
        });
    }

    handleDayChange(day, { selected }) {
        if (selected) {
            // Unselect the day if already selected
            this.setState({ startDate: undefined }, () => {
                this.props.tjenesteplanConfigChanged(this.state);
            });
        } else {
            this.setState({ startDate: day }, () => {
                this.props.tjenesteplanConfigChanged(this.state);
            });
        }
      }

    handleSubmit() {
        if(this.props.history.location.pathname.endsWith('/')) {
            history.push(this.props.location.pathname + 'ukeplaner');
        }else {
            history.push(this.props.location.pathname + '/ukeplaner');
        }
    }

    render() {
        return (
            <div className="new-tjenesteplan-config">
                <div className="absolute-position fill-height fill-width">
                    <div className="container fill-height fill-width">
                        <div className="row align-items-center justify-content-center fill-height fill-width">
                            <div className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                                <form>
                                    <div className="form-group">
                                        <label htmlFor="name">Navn</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            name="name"
                                            value={this.state.name}
                                            onChange={this.handleInputChange}
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="startDate">Startdato</label>

                                        <DayPicker
                                            onDayChange={this.handleDayChange}
                                            selectedDays={this.state.startDate}
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="numberOfLeger">Antall leger</label>
                                        <select
                                            className="form-control"
                                            name="numberOfLeger"
                                            value={this.state.numberOfLeger}
                                            onChange={this.handleInputChange}
                                        >
                                        {Array(20).fill().map((n, index) => (
                                            <option>{index+1}</option>
                                        ))}

                                        </select>
                                    </div>
                                    <button
                                        type="button"
                                        className="btn btn-primary"
                                        onClick={this.handleSubmit}
                                    >
                                        Fortsett
                                    </button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        sykehus: state.sykehus.sykehus,
        config: state.createTjenesteplanForm.config
    };
}

function mapDispatchToProps(dispatch) {
    return {
        initialize: () => {
            return dispatch(sykehusApi.actions.getSykehus());
        },

        initBreadcrumbs: (sykehus, avdeling) => {
            dispatch(
                breadcrumbActions.set(
                  [
                    { name: "Sykehus", link: "/sykehus"},
                    { name: sykehus.name, link: "/sykehus/" + sykehus.id },
                    { name: avdeling.name, link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id },
                    { name: "Tjenesteplaner", link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id + '/tjenesteplaner' }
                  ]
                )
            );
        },

        tjenesteplanConfigChanged: (config) => {
            dispatch(actions.tjenesteplanConfigChanged(config));
        }
    }
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(CreateTjenesteplanConfig));