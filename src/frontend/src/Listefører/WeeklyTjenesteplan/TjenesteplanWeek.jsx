import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import { dagsplanConstants } from '../../_constants/dagsplan.constants';

class TjenesteplanWeek extends React.Component {

  constructor(props) {
    super(props);
    this.currentWeekRef = React.createRef();
  }

  render() {
    const { week, dagsplaner, selectedDay, handleDayClick, scrollToWeek } = this.props;

    return (
        <div>
            <Week
                leger={this.props.leger}
                week={week}
                dagsplaner={dagsplaner}
                selectedDay={selectedDay}
                handleDayClick={handleDayClick}
            />
        </div>
    );
  }
}


function Week(props){
    const { selectedDay, week, dagsplaner, handleDayClick, leger } = props;

    let header = "Uke " + week.weekNr;
    if(week.weekNr === moment().week()) {
      header += " - Inneværende uke"
    }

    return (
      <div>
        <h2>{header}</h2>

        <div className="table-scroll">
          <table className="table">
          <thead className="thead-light">
              <tr>
                  <th className="new-tjenesteplan-date sticky-column">Dato</th>
                  <th className="new-tjenesteplan-day sticky-column">Dag</th>
                  {week.legeWeeks.map((legeWeek, index) => {
                      return (
                          <th key={index}>
                              {legeWeek.lege.fullname}
                          </th>
                      )
                  })}

              </tr>
          </thead>
          <tbody>
              {week.legeWeeks[0].dager.map((day) => {
                const date = moment(day.date);

                let dayClassName = "sticky-column";
                let dayName = date.format('dddd');
                if(day.isHoliday) {
                  dayClassName += " holiday";
                  dayName = day.holidayDescription;
                }

                // If is today
                if(date.isSame(new Date(), "day")) {
                  dayClassName += " today";
                }

                return (
                  <tr key={day.date}>
                      <td key="date" className={dayClassName+" new-tjenesteplan-date"}>
                        {date.format('DD.MM')}
                      </td>
                      <td key="day" className={dayClassName + " new-tjenesteplan-day"}>
                        {dayName}
                      </td>
                      {week.legeWeeks.map((legeWeek, index) => {
                          // More efficient comparison of correct date (ignoring time) than creating moment instances
                          const weekDay = legeWeek.dager.find(d => d.date.substring(0, 10) === day.date.substring(0, 10));
                          if(!weekDay){
                            throw "Could not find weekday for date " + day.date;
                          }
                          return Weekday(
                              index,
                              legeWeek,
                              dagsplaner,
                              weekDay,
                              selectedDay,
                              handleDayClick
                          );
                        }
                      )}
                  </tr>
                );
              })}
          </tbody>
        </table>
      </div>
    </div>
    )
  }


function Weekday(index, week, dagsplaner, weekday, selectedDay, handleDayClick) {
  const attr = {};
  attr.key = week.weekNr + "-" + weekday.date + "-" + week.userId;
  attr.className = "datepickerDay--dagsplan-" + weekday.dagsplan;
  attr.onClick = () => handleDayClick(week, weekday);

  // If is selected
  if(selectedDay &&
    selectedDay.week.userId === week.userId &&
    selectedDay.week.weekNr === week.weekNr &&
    moment(weekday.date).isSame(selectedDay.day.date, 'day')) {
    attr.className += " weekday--selected";
  }

  let name = "";
  const dagsplan = dagsplaner.find(c => c.id === weekday.dagsplan);
  if(dagsplan && dagsplan.id !== dagsplanConstants.None) {
      name = dagsplan.name;
  }

  return (
      <td {...attr}>{name}</td>
  )
}

export default TjenesteplanWeek;