import React from 'react';
import { connect } from 'react-redux';

function day(date, isPrevMonth){
    if(!isPrevMonth){
        return (<span>{date}</span>);
    }

    return (<span/>)
}

function datepickerDay(isSelectable, dagsplan, date, cssClasses, isPrevMonth, onClick, onMouseEnter){
    let title = '';
    if(dagsplan) {
        title = dagsplan.name;
    }

    let attr = {
        className: cssClasses,
        title: title
    };

    if(isSelectable) {
        attr.onClick = () => onClick(date, dagsplan);
        attr.onMouseEnter = () => onMouseEnter(date);
    } else {
        attr.onMouseEnter = () => onMouseEnter(date);
    }

    return (
        <div className="wrapper">
            <div
                {...attr}
            >
                {day(date.date(), isPrevMonth)}
            </div>
        </div>
    );
}

class DatePickerDay extends React.Component {
    constructor(props) {
        super(props);
        this.state = { isHighlighted: false, dagsplanId: null };
      }

    handleSelectMouseOver() {
        if( this.props.selectStart !== null &&
            this.props.day > this.props.selectStart) {
            this.setState( { isHighlighted: true });
        }
    }

    handleSelectMouseOut(){
        if(this.state.isHighlighted === true && this.props.day < this.props.selectStart) {
            this.setState({ isHighlighted: false });
        }
    }

    shouldComponentUpdate(nextProps, nextState) {
        if( nextProps.day !== this.props.day ||
            nextProps.dagsplan !== this.props.dagsplan ||
            nextProps.isHoliday !== this.props.isHoliday ||
            nextProps.isSelectable !== this.props.isSelectable ||
            nextProps.isSelected !== this.props.isSelected ||
            nextState.isHighlighted !== this.state.isHighlighted
        ) {
            return true;
        }

        return false;
    }

    render() {

        const date = this.props.moment;
        const isPrevMonth = this.props.isPrevMonth;
        const dagsplan = this.props.dagsplan;
        const isSelectable = this.props.isSelectable;
        const isSelected = this.props.isSelected || false;
        const isMarked = this.props.isMarked || false;

        let cssClasses = "datepickerDay"
        if(isSelectable && !isSelected) {
            cssClasses += " datepickerDay--selectable";
        }

        if(isSelected){
            cssClasses += " datepickerDay--selected";
        }

        if(isMarked) {
            cssClasses += " datepickerDay--marked";
        }

        if(this.props.selectStart !== null &&
            date.isAfter(this.props.selectStart) &&
            date.isSameOrBefore(this.props.highlightDate) &&
            !isPrevMonth) {

                cssClasses += " datepickerDay--highlighted";
                return datepickerDay(
                    isSelectable,
                    dagsplan,
                    date,
                    cssClasses,
                    isPrevMonth,
                    day => this.props.onClick(day, dagsplan),
                    day => this.props.onSelectMouseOver(day)
                );
        }

        if(dagsplan !== null && !isPrevMonth) {
            cssClasses += " datepickerDay-dagsplan datepickerDay--dagsplan-" + dagsplan.id;

            if(this.props.isHoliday) {
                cssClasses += " datepickerDay--helligdag";
            }

            return datepickerDay(
                isSelectable,
                dagsplan,
                date,
                cssClasses,
                isPrevMonth,
                day => this.props.onClick(day, dagsplan),
                day => this.props.onSelectMouseOver(day)
            );
        }

        if(this.props.isHoliday && !isPrevMonth) {
            cssClasses += " datepickerDay--helligdag";

            return datepickerDay(
                isSelectable,
                dagsplan,
                date,
                cssClasses,
                isPrevMonth,
                day => this.props.onClick(day, dagsplan),
                day => this.props.onSelectMouseOver(day)
            );
        }

        if(isPrevMonth){
            cssClasses += " datepickerDay--prevmonth";
            return datepickerDay(
                isSelectable,
                dagsplan,
                date,
                cssClasses,
                isPrevMonth,
                () => {},
                () => {}
            );
        }

        cssClasses += " datepickerDay--unselected";
        return datepickerDay(
            isSelectable,
            dagsplan,
            date,
            cssClasses,
            isPrevMonth,
            day => this.props.onClick(day, dagsplan),
            day => this.props.onSelectMouseOver(day)
        );
    }
}

function mapStateToProps(state) {
    return {
    };
}

const connectedDatePickerDay = connect(mapStateToProps)(DatePickerDay);
export { connectedDatePickerDay as DatePickerDay };