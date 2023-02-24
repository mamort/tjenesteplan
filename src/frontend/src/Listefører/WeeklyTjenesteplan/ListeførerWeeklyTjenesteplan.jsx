import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import tjenesteplaner from '../../Api/Tjenesteplaner';
import { dagsplanerActions } from '../../Api/Dagsplaner';
import TjenesteplanWeek from './TjenesteplanWeek';
import Leger from '../../Tjenesteplan/Leger';
import { Loader } from '../../_components/Loader.jsx';
import scroll from '../../_helpers/scroll';

class ListeførerWeeklyTjenesteplan extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      selectedDay: null,
      displayDagsplanEditActions: false,
      selectedDagsplan: null,
      weeks: []
    };

    this.updateWeeks = this.updateWeeks.bind(this);
    this.scrollToCurrentWeek = this.scrollToCurrentWeek.bind(this);
    this.handleDayClick = this.handleDayClick.bind(this);
    this.handleEditDagsplanClick = this.handleEditDagsplanClick.bind(this);
    this.handleChangeDagsplan = this.handleChangeDagsplan.bind(this);
    this.handleSaveDagsplanChange = this.handleSaveDagsplanChange.bind(this);

    this.currentWeekRef = React.createRef();
  }

  componentDidMount() {
    const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);

    this.setState({
      shouldScrollToWeek: true
    });

    if(tjenesteplanId) {
      this.props.initialize(tjenesteplanId)
        .then(() => {
          this.updateWeeks(
            () => {
              this.scrollToCurrentWeek();
            }
          );
        });
    }
  }

  scrollToCurrentWeek () {
    if(this.currentWeekRef.current) {
        const offset = this.currentWeekRef.current.offsetTop;
        setTimeout(function () {
            window.scrollTo({ top: offset, behavior: 'smooth' });
        },2);
    }
  }

  updateWeeks(callback) {
    const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
    const weeklyTjenesteplan = this.props.tjenesteplaner[tjenesteplanId];

    const weeks = [];

    for(let weekNr = 1; weekNr < 55; weekNr++){
      const legeWeeks = [];
      for(let i = 0; i < weeklyTjenesteplan.weeks.length; i++){
        const week = weeklyTjenesteplan.weeks[i];
        if(week.weekNr === weekNr) {
          const lege = this.props.leger.find(l => l.id === week.userId);
          legeWeeks.push({ ...week, lege });
        }
      }

      if(legeWeeks.length > 0){
        weeks.push({ weekNr, legeWeeks });
      }
    }

    this.setState({
      weeks
    }, callback);
  }

  handleEditDagsplanClick(evt) {
    evt.preventDefault();

    const displayActions = this.state.displayDagsplanEditActions
    const self = this;

    this.setState({
      displayDagsplanEditActions: !displayActions
    },
    () => {
      if(displayActions) {
        setTimeout(function() {
          if(!self.state.displayDagsplanEditActions){
            self.setState({selectedDagsplan: null});
          }
        }, 1000);
      }
    });
  }

  handleDayClick(week, weekDay) {
    const lege = this.props.leger.find(l => l.id === week.userId);
    this.setState({
      displayDagsplanEditActions: false,
      selectedDagsplan: null,
      selectedDay: {
        lege: lege,
        week: week,
        day: weekDay,
        moment: moment(weekDay.date)
      }
    });
  }

  handleChangeDagsplan(dagsplan) {
    this.setState({
      selectedDagsplan: dagsplan
    });
  }

  handleSaveDagsplanChange() {
    const selectedDagsplan = this.state.selectedDagsplan;
    const selectedDay = this.state.selectedDay;
    if(selectedDagsplan && selectedDay) {
      var topScrollPosition = scroll.getTopScrollPosition();

      const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);

      this.props.changeTjenesteplanDate(
        tjenesteplanId,
        selectedDay.lege.id,
        selectedDay.moment,
        selectedDagsplan
      ).then(() => {
        this.updateWeeks(() => {
          window.scrollTo({ top: topScrollPosition });
        });
      });

      this.setState({
        displayDagsplanEditActions: false,
        shouldScrollToWeek: false
      });
    }
  }


  render() {
    const tjenesteplanId = this.props.match.params.tjenesteplanId;
    const weeklyTjenesteplan = this.props.tjenesteplaner[tjenesteplanId];

    if(this.props.isFetching || !weeklyTjenesteplan) {
      return <Loader />
    }

    const editableDagsplaner =
      this.props.dagsplaner.filter(d => !d.isSystemDagsplan);

    editableDagsplaner.push({ id: 0, name: "Vanlig arbeidsdag" });

    let dayText = "";
    let legeName = "";
    let isDaySelected = false;
    if(this.state.selectedDay) {
      dayText = this.state.selectedDay.moment.format("Do MMMM");
      isDaySelected = true;
      legeName = this.state.selectedDay.lege.fullname;
    }

    const currentWeekNr = moment().week();

    return (
      <div className="weeklyTjenesteplan">
        <h1>Løpende tjenesteplan for {weeklyTjenesteplan.year}</h1>

        {this.state.weeks.map((week) => {
              const props = {};
              if(week.weekNr === currentWeekNr) {
                  props.ref = this.currentWeekRef;
              }

              return (<div {...props} key={week.weekNr}>
                  <TjenesteplanWeek
                    week={week}
                    leger={this.props.leger}
                    dagsplaner={this.props.dagsplaner}
                    selectedDay={this.state.selectedDay}
                    handleDayClick={this.handleDayClick}
                  />
                </div>
              )

          }
        )}
        <div className="weeklyTjenesteplan-footer-menu">
              <div className="weeklyTjenesteplan-footer-menu-spacer"></div>

              <ul className={
                  this.state.displayDagsplanEditActions
                      ? "weeklyTjenesteplan-footer-actionitems weeklyTjenesteplan-footer-actionitems--show"
                      : "weeklyTjenesteplan-footer-actionitems"
                  }
              >
                  <li>Endre dagsplan for:<br /> {legeName}</li>

                  { this.state.selectedDagsplan
                    ? <EditDagsplan
                        selectedDagsplan={this.state.selectedDagsplan}
                        currentDagsplanId={this.state.selectedDay.day.dagsplan}
                        dagsplaner={this.props.dagsplaner}
                        handleSaveDagsplanChange={this.handleSaveDagsplanChange}
                      />
                    : <PickDagsplan
                        editableDagsplaner={editableDagsplaner}
                        handleChangeDagsplan={this.handleChangeDagsplan}
                      />
                  }
              </ul>

              <div className="weeklyTjenesteplan-footer-menu-header">
                  <div className={
                      isDaySelected
                      ? "weeklyTjenesteplan-footer-edit weeklyTjenesteplan-footer-edit--selected"
                      : "weeklyTjenesteplan-footer-edit"
                    }>
                    <span>{dayText}</span>
                    <div className="footer-spacer-left"></div>
                    <a href="#" onClick={(evt) => this.handleEditDagsplanClick(evt)}>
                      <i className={
                        this.state.displayDagsplanEditActions
                        ? "dripicons-cross"
                        : "dripicons-document-edit"
                      }></i>
                      <span>Endre dagsplan</span>
                    </a>
                  </div>
              </div>
          </div>
      </div>
    )
  }
}

function PickDagsplan(props) {
  const { editableDagsplaner, handleChangeDagsplan} = props;
  return (
    editableDagsplaner.map(d => {
      return (
        <li
          key={d.id}
          className={"datepickerDay--dagsplan-" + d.id}
          onClick={() => handleChangeDagsplan(d)}
        >
          {d.name}
        </li>
      );
    })
  );
}

function EditDagsplan(props) {
  const {
    currentDagsplanId,
    selectedDagsplan,
    dagsplaner,
    handleSaveDagsplanChange
  } = props;

  let currentDagsplan = "Vanlig arbeidsdag";
  if(currentDagsplanId > 0) {
    currentDagsplan = dagsplaner.find(d => d.id === currentDagsplanId).name;
  }

  return (
    <li className="weeklyTjenesteplan-footer-actionitems--save">
      <div>
        <span>{currentDagsplan}</span>
        <i className="dripicons-arrow-thin-right" />
        <span>{selectedDagsplan.name}</span>
      </div>
      <button className="btn btn-primary" onClick={() => handleSaveDagsplanChange()}>Lagre</button>
    </li>
  );
}

function mapStateToProps(state) {
    return {
      isFetching: state.weeklyTjenesteplan.isFetching || state.users.loading,
      tjenesteplaner: state.weeklyTjenesteplan.tjenesteplaner,
      dagsplaner: state.dagsplaner.alleDagsplaner,
      leger: state.users.leger
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: (tjenesteplanId) => {
      return Promise.all([
        dispatch(dagsplanerActions.getDagsplaner()),
        dispatch(tjenesteplaner.actions.getWeeklyTjenesteplan(tjenesteplanId)),
        dispatch(Leger.actions.getLeger())
      ]);
    },

    changeTjenesteplanDate: (tjenesteplanId, legeId, date, dagsplan) => {
      return dispatch(tjenesteplaner.actions.changeTjenesteplanDateForLege(tjenesteplanId, legeId, date, dagsplan))
        .then(() => dispatch(tjenesteplaner.actions.getWeeklyTjenesteplan(tjenesteplanId)));
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(ListeførerWeeklyTjenesteplan);