import React from 'react';
import moment from 'moment';
import { connect } from 'react-redux';
import { DatePickerDay } from './DatePickerDay'

class DatePickerMonth extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            currentMonthMoment: moment.utc(),
            weeks: []
        };

        this.shouldUpdate = this.shouldUpdate.bind(this);
    }

    componentDidMount() {
        this.updateWeeks(this.props);
    }

    componentWillReceiveProps(newProps) {
        if(this.shouldUpdate(newProps, this.props)) {
            this.updateWeeks(newProps);
        }
    }

    shouldUpdate(newProps, oldProps) {
        if(newProps.dates !== oldProps.dates) {
            const newDays = newProps.dates;
            const oldDays = oldProps.dates;

            if(newDays.length !== oldDays.length) {
                return true;
            } else {
                for(let i = 0; i < oldDays.length; i++) {
                    if(!newDays.includes(oldDays[i])) {
                        return true;
                    }
                }

                for(let i = 0; i < newDays.length; i++) {
                    if(!oldDays.includes(newDays[i])) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    shouldComponentUpdate(nextProps, nextState) {
        if(this.shouldUpdate(nextProps, this.props)){
            return true;
        }

        if(nextState.weeks !== this.state.weeks) {
            return true;
        }

        return false;
    }

    updateWeeks(props) {
        const prevMonthDate = Date.UTC(props.year, props.month, 0);
        const currentMonthDate = Date.UTC(props.year, props.month);
        const currentMonthMoment = moment.utc(currentMonthDate);
        const firstWeek =  moment.utc(currentMonthDate).week();
        const newDatesForMonth = props.dates;

        let dateIterator = moment.utc(prevMonthDate);
        let days = [];
        while(dateIterator.week() === firstWeek){
            days.unshift({ moment: dateIterator.clone(), date: dateIterator.date(), isPrevMonth: true });
            dateIterator = dateIterator.add(-1, 'days');
        }

        dateIterator = currentMonthMoment.clone();

        while(dateIterator.month() === currentMonthMoment.month()) {
            const date = dateIterator.clone();
            const dagsplanDay = this.findDagsplanDay(newDatesForMonth, date);
            const dagsplan = this.findDagsplan(
                props.selectStart,
                props.selectedDagsplan,
                dagsplanDay
            );

            let isHoliday = false;
            let isSelected = false;
            let description = "";
            if(dagsplanDay) {
                isHoliday = dagsplanDay.isHoliday;
                description = dagsplanDay.description;
                isSelected = dagsplanDay.isSelected || false;
            }

            days.push({
                moment: date,
                date: dateIterator.date(),
                dagsplan: dagsplan,
                isPrevMonth: false,
                isHoliday: isHoliday,
                isSelected: isSelected,
                isSelectable: props.isDateSelectable(
                    date,
                    dagsplan,
                    isHoliday,
                    dagsplanDay
                ),
                isMarked: props.isDateMarked ? props.isDateMarked(dagsplanDay) : false,
                description: description
            });
            dateIterator = dateIterator.add(1, 'days');
        }

        const weeks = [];
        let currentWeekNr = days[0].moment.week();
        let week = { weekNr: currentWeekNr, days: []};
        for(let i = 0; i < days.length; i++){
            const day = days[i];
            let weekNr = day.moment.week();

            if(currentWeekNr != weekNr) {
                weeks.push(week);
                currentWeekNr = weekNr;
                week = { weekNr: currentWeekNr, days: []};
            }

            week.days.push(day);
        }

        weeks.push(week);

        this.setState({
            currentMonthMoment: currentMonthMoment,
            days: days,
            weeks: weeks
        });
    }

    getDagsplan(id) {
        for(let i = 0; i < this.props.dagsplaner.length; i++) {
            const dagsplan = this.props.dagsplaner[i];
            if(dagsplan.id === id) {
                return dagsplan;
            }

            if(dagsplan.followedBy) {
                for(let k = 0; k < dagsplan.followedBy.length; k++) {
                    const subDagsplan = dagsplan.followedBy[k];
                    if(subDagsplan.id === id) {
                        return subDagsplan;
                    }
                }
            }
        }

        return null;
    }

    isSameDate(d1, d2){
        return d1.year() === d2.year() &&
        d1.month() === d2.month() &&
        d1.date() === d2.date()
    }

    findDagsplanDay(dates, currentDate) {
        for(let i = 0; i < dates.length; i++) {
            let date = dates[i];
            if(this.isSameDate(currentDate, date.date)) {
                return date;
            }
        }

        return null;
    }

    findDagsplan(selectStart, selectedDagsplan, dagsplanDay) {
        if(dagsplanDay == null) {
            return null;
        }

        return this.getDagsplan(dagsplanDay.dagsplanId);
    }

    render() {
        return (
            <div className="datepickerMonth col-xs-12 col-sm-12 col-md-6 col-lg-4">
                <h2>{this.state.currentMonthMoment.format("MMMM YYYY")}</h2>

                <table>
                    <thead>
                        <tr>
                            <th></th>
                            <th>Ma</th>
                            <th>Ti</th>
                            <th>On</th>
                            <th>To</th>
                            <th>Fr</th>
                            <th>Lø</th>
                            <th>Sø</th>
                        </tr>
                    </thead>
                    <tbody>
                    {this.state.weeks.map((week, index) => (
                        <tr key={index}>
                            <td><div className="weekNr">{week.weekNr}</div></td>
                            {week.days.map((day, index) => (
                                <td key={day.moment.format()}>
                                    <DatePickerDay
                                        moment={day.moment}
                                        day={day.date}
                                        isPrevMonth={day.isPrevMonth}
                                        isHoliday={day.isHoliday}
                                        isSelected={day.isSelected}
                                        isSelectable={day.isSelectable}
                                        isMarked={day.isMarked}
                                        description={day.description}
                                        dagsplan={day.dagsplan}
                                        onClick={(date, dagsplan) => this.props.handleDateClick(date, dagsplan)}
                                        onSelectMouseOver={ date => this.props.handleSelectDateHover(date)}
                                        highlightDate={this.props.highlightDate}
                                        selectStart={this.props.selectStart}
                                    />
                                </td>
                            ))}
                        </tr>
                    ))}
                    </tbody>
                </table>

            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
    };
}

const connectedDatePickerMonth = connect(mapStateToProps)(DatePickerMonth);
export { connectedDatePickerMonth as DatePickerMonth };