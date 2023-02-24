import React from 'react';
import { connect } from 'react-redux';
import Select from 'react-select';
import moment from 'moment';
import { Loader } from '../_components/Loader.jsx';
import * as actions from './tjenesteplan.actions';
import DagsplanPicker from './DagsplanPicker';
import { dagsplanConstants } from '../_constants/dagsplan.constants';
import tjenesteplaner from '../Api/Tjenesteplaner';
import Leger from './Leger';
import { breadcrumbActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';

class CreateTjenesteplanWeeks extends React.Component {

    constructor(props) {
        super(props);

        const weeks = [];

        for(let i = 0; i < this.props.config.numberOfLeger; i++) {
            weeks.push({ id: i, name: "Velg lege", days: [] });
        }

        this.state = {
            weeks,
            selectedDate: null,
            selectedWeek: null,
            displayDagsplanPicker: false,
            isSaving: false,
            dagsplaner : [
                { id: 1, name: "Vakt" },
                { id: 2, name: "Fri etter vakt" },
                { id: 3, name: "Permisjon" },
                { id: 4, name: "Ferie" }
              ]
        };

        this.closeDagsplanPicker = this.closeDagsplanPicker.bind(this);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.handleLegeChange = this.handleLegeChange.bind(this);
        this.saveChanges = this.saveChanges.bind(this);
        this.handleNameChange = this.handleNameChange.bind(this);
    }

    componentDidMount() {
        const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
        const avdelingId = parseInt(this.props.match.params.avdelingId, 10);
        const tjenesteplanId = parseInt(this.props.match.params.id, 10);

        if(tjenesteplanId) {
            this.props.initializeEditTjenesteplan(tjenesteplanId)
                .then(() => {
                    const tjenesteplanInfo = this.props.tjenesteplaner[tjenesteplanId];

                    this.setState({
                        name: tjenesteplanInfo.name
                    });
                });
        } else {
            if(!this.props.config.startDate) {
                if(this.props.history.location.pathname.endsWith('/')) {
                    this.props.history.push('..');
                } else {
                    this.props.history.push('.');
                }
            }else{
                this.setState({
                    avdelingId: avdelingId,
                    startDate: this.props.config.startDate,
                    name: this.props.config.name,
                    numberOfLeger: this.props.numberofLeger
                });
            }
        }

        this.props.initialize()
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
                this.props.initBreadcrumbs(sykehus, avdeling);
            });
    }

    componentWillReceiveProps(newProps) {
        const tjenesteplanId = parseInt(this.props.match.params.id, 10);
        if(tjenesteplanId){
            const tjenesteplanInfo = newProps.tjenesteplaner[tjenesteplanId];

            if(tjenesteplanInfo) {
                const weeks = [];

                for(let i = 0; i < tjenesteplanInfo.weeks.length; i++) {
                    const weekInfo = tjenesteplanInfo.weeks[i];
                    const days = [];

                    for(let k = 0; k < weekInfo.days.length; k++) {
                        const dayInfo = weekInfo.days[k];
                        days.push({ date: moment.utc(dayInfo.date).clone(), dagsplan: dayInfo.dagsplan });
                    }

                    let legeId = null;
                    if(weekInfo.lege) {
                        legeId = weekInfo.lege.id;
                    }

                    weeks.push({ id: weekInfo.id, legeId: legeId, days: days });
                }

                this.setState({
                    id: tjenesteplanInfo.id,
                    startDate: moment.utc(tjenesteplanInfo.startDate).clone(),
                    name: tjenesteplanInfo.name,
                    numberOfLeger: tjenesteplanInfo.numberofLeger,
                    weeks: weeks
                });
            }
        }
    }

    cancelDagsplanPicker() {
        this.setState({
            displayDagsplanPicker: false,
            selectedDate: null,
            selectedWeek: null
        });
    }

    closeDagsplanPicker() {
        this.setState({
            displayDagsplanPicker: false,
            selectedDate: null,
            selectedWeek: null
        });
    }

    handleNameChange(event) {
        const { name, value } = event.target;

        this.setState({
            name: value
        });
    }

    handleDayClick(week, date) {
        this.setState({
            selectedDate: date,
            selectedWeek: week,
            displayDagsplanPicker: true
        });
    }

    handleDagsplanSelected(dagsplan) {
        const weekId = this.state.selectedWeek;
        const date = this.state.selectedDate;

        const newWeeks = [...this.state.weeks];
        const week = newWeeks.find(w => w.id == weekId);
        const day = week.days.find(d => d.date.isSame(date, 'day'));

        if(dagsplan > 0){
            if(day) {
                day.dagsplan = dagsplan;
            } else {
                week.days.push({ date, dagsplan })
            }
        }else{
            week.days.splice( week.days.indexOf(day), 1 );
        }

        this.setState({
            weeks: newWeeks
        });

        this.closeDagsplanPicker();
    }

    handleLegeChange(weekId, legeId) {
        const newWeeks = [...this.state.weeks];

        for(let i = 0; i < newWeeks.length; i++){
            const w = newWeeks[i];
            if(w.legeId === legeId){
                w.legeId = -1;
            }
        }

        const week = newWeeks.find(w => w.id === weekId);
        week.legeId = legeId;

        this.setState({
            weeks: newWeeks
        });
    }

    saveChanges() {
        const component = this;
        component.setState({
            isSaving:true
        }, () => {
            component.props.save(
                this.state.avdelingId,
                this.state.id,
                this.state.startDate,
                this.state.name,
                this.state.numberOfLeger,
                this.state.weeks
            ).then(() => {
                component.setState({
                    isSaving:false
                });
            });
        });
    }

    render() {
        const avdelingId = parseInt(this.props.match.params.avdelingId, 10);
        const tjenesteplanId = parseInt(this.props.match.params.id, 10);

        let leger = [];
        if(this.props.leger) {
            for(let i = 0; i < this.props.leger.length; i++) {
                const lege = this.props.leger[i];

                if(lege.avdelinger.find(id => id == avdelingId)){
                    leger.push(lege);
                }
            }
        }

        if(!this.state.startDate || this.state.isSaving) {
            return <Loader/>;
        }

        const days = [];
        let currentDay = moment.utc(this.state.startDate).clone();

        for(let i = 0; i < 7; i++) {
            const weeksForDate = [];
            for(let k = 0; k < this.state.weeks.length; k++) {
                let week = this.state.weeks[k];

                let found = false;
                for(let d = 0; d < week.days.length; d++) {
                    let day = week.days[d];

                    if(day.date.isSame(currentDay, 'day')) {
                        weeksForDate.push( { week: week.id, date: currentDay.clone(), dagsplan: day.dagsplan });
                        found = true;
                    }
                }

                if(!found) {
                    weeksForDate.push( { week: week.id, date: currentDay.clone(), dagsplan: 0 })
                }
            }

            days.push({ date: currentDay.clone(), weeks: weeksForDate });

            currentDay = currentDay.add(1, 'days');
        }


        return (
            <div>
                <div className="container bottom-header-spacing new-tjenesteplan">
                    <TjenesteplanHeader
                        tjenesteplanId={tjenesteplanId}
                        tjenesteplanName={this.state.name}
                        handleNameChange={this.handleNameChange}
                    />

                    <DagsplanPicker
                        isOpen={this.state.displayDagsplanPicker}
                        date={this.state.selectedDate}
                        onClose={() => this.closeDagsplanPicker() }
                        onCancel={() => this.cancelDagsplanPicker()}
                        onDagsplanSelected={ c => this.handleDagsplanSelected(c) }
                    />

                    <div className="table-scroll">
                    <table className="table">
                        <thead className="thead-light">
                            <tr>
                                <th className="new-tjenesteplan-date sticky-column">Dato</th>
                                <th className="new-tjenesteplan-day sticky-column">Dag</th>
                                {this.state.weeks.map((week, index) => (
                                    <th key={index}>
                                        <LegePicker
                                            onLegeChange={this.handleLegeChange}
                                            leger={leger}
                                            week={week}
                                            weeks={this.state.weeks}
                                        />
                                    </th>
                                ))}

                            </tr>
                        </thead>
                        <tbody>

                            {days.map((day) => (
                            <tr key={day.date.format()}>
                                <td key="date" className="new-tjenesteplan-date sticky-column">{day.date.format('DD.MM.YYYY')}</td>
                                <td key="day" className="new-tjenesteplan-day sticky-column">{day.date.format('dddd')}</td>
                                {day.weeks.map((weekday, index) =>
                                    Weekday(
                                        index,
                                        this.props.dagsplaner,
                                        weekday,
                                        this.handleDayClick
                                    )
                                )}
                            </tr>
                            ))}
                        </tbody>
                    </table>
                    </div>
                </div>
                <div className="bottom-header">
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={this.saveChanges}
                    >
                            Lagre tjenesteplan
                    </button>
                    <SaveLoader isSaving={this.props.isSaving}/>
                </div>
            </div>
        );
    }
}

function SaveLoader(props){
    const { isSaving } = props;
    if(!isSaving){
        return (<div className="loader-small disabled" />);
    }

    return(
        <div className="loader-small"></div>
    );
}

function TjenesteplanHeader(props) {
    const { tjenesteplanId, tjenesteplanName, handleNameChange } = props;

    if(tjenesteplanId) {
        return (<div className="change-tjenesteplan-name">
            <h1>Endre tjenesteplan</h1>
            <form name="form">
                <div className="form-group">
                    <label htmlFor="title">Navn</label>
                    <input
                      type="text"
                      className="form-control"
                      name="title"
                      value={tjenesteplanName}
                      onChange={handleNameChange}
                    />
                </div>
            </form>
        </div>);
    }

    return (<h1>Ny tjenesteplan</h1>);
}

function LegePicker(props) {
    const { weeks, leger, week, onLegeChange } = props;

    if(leger.length === 0) {
        return (
            <span>Ingen tilgjengelige leger å velge</span>
        );
    }

    const legerOptions = leger.filter((l) => l.id !== week.legeId);
    let options = legerOptions.map(lege => {
        let label = lege.fullname;
        const week = weeks.find(w => w.legeId == lege.id);
        if(week) {
            label += " (tildelt uke: " + week.id + ")";
        }
        return { value: lege.id, label: label };
    });

    const lege = leger.find(l => l.id === week.legeId);

    let selectedValue = { value: -1, label: "Velg lege" };
    if(lege){
        selectedValue = { value: week.legeId, label: lege.fullname };
        options.push({ value: -1, label: "Ingen lege" });
    }else{
        options.push(selectedValue);
    }

    return (
        <Select
            name={"week-" + week.id}
            className="new-tjenesteplan-legePicker"
            defaultValue={selectedValue}
            value={selectedValue}
            options={options}
            onChange={event => onLegeChange(week.id, event.value)}
            hideSelectedOptions={true}
        />
    )
}

function Weekday(index, dagsplaner, weekday, handleDayClick) {
    const attr = {};
    attr.key = index;
    attr.className = "datepickerDay--dagsplan-" + weekday.dagsplan;
    attr.onClick = () => handleDayClick(weekday.week, weekday.date);

    let name = "";
    const dagsplan = dagsplaner.find(c => c.id === weekday.dagsplan);
    if(dagsplan && dagsplan.id !== dagsplanConstants.None) {
        name = dagsplan.name;
    }

    return (
        <td {...attr}>{name}</td>
    )
}

function mapStateToProps(state) {
    return {
        sykehus: state.sykehus.sykehus,
        config: state.createTjenesteplanForm.config,
        tjenesteplaner: state.tjenesteplanInfo.tjenesteplaner,
        isSaving: state.tjenesteplaner.isFetching,
        dagsplaner: state.dagsplaner.alleDagsplaner,
        leger: state.users.leger
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      initialize: () => {
        dispatch(actions.initialize());
        dispatch(Leger.actions.getLeger());
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

      initializeEditTjenesteplan: (tjenesteplanId) => {
        return dispatch(tjenesteplaner.actions.getTjenesteplan(tjenesteplanId));
      },

      save: (avdelingId, id, startDate, name, numberOfLeger, weeks) => {
        for(let i = 0; i < weeks.length; i++) {
            const week = weeks[i];
            if(week.legeId == -1) {
                week.legeId = null;
            }
        }

        return dispatch(tjenesteplaner.actions.saveTjenesteplan(
            avdelingId,
            id,
            name,
            moment.utc(startDate),
            numberOfLeger,
            weeks
        )).then(function(){
            if(id) {
                return Promise.all([
                    dispatch(Leger.actions.getLeger()),
                    dispatch(tjenesteplaner.actions.getTjenesteplan(id)),
                ]);
            }

            return Promise.resolve();
        });
      }
    }
  }

export default connect(mapStateToProps, mapDispatchToProps)(CreateTjenesteplanWeeks);