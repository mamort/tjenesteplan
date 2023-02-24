import React from 'react';
import { connect } from 'react-redux';
import { DatePickerMonth } from './DatePickerMonth';

class DatePicker extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            highlightDate: null
        };
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.selectStart !== this.props.selectStart && nextProps.selectStart === null) {
          this.setState({
            highlightDate: null
          });
        }
      }

    handleDateClick(moment, dagsplan){
        if(this.props.selectStart === null) {
            this.props.onSelectStartDate(moment, dagsplan);
        } else if( moment.isSameOrAfter(this.props.selectStart, 'day')){
            this.setState({ highlightDate: null });
            this.props.onSelectEndDate(this.props.selectStart, moment, this.props.selectedDagsplan);
        } else if(this.props.selectStart.isSame(moment, 'day')) {
            this.setState({ highlightDate: null });
        } else {
            this.setState({ highlightDate: null });
            this.props.onSelectStartDate(moment);
        }
    }

    handleSelectDateHover(date) {
        if( this.props.selectStart !== null &&
            date.isSameOrAfter(this.props.selectStart, 'day')) {

            this.setState( { highlightDate: date });
        }
    }

    render() {
        const year = new Date().getFullYear();

        // Javascript måned starter på 0. Januar = 0
        const months = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        return (
            <div className="row">
                {months.map((month, index) => (
                    <DatePickerMonth
                        year={year}
                        month={month}
                        dagsplaner={this.props.dagsplaner}
                        isDateSelectable={this.props.isDateSelectable}
                        isDateMarked={this.props.isDateMarked}
                        selectStart={this.props.selectStart}
                        selectedDagsplan={this.props.selectedDagsplan}
                        highlightDate={this.state.highlightDate}
                        dates={this.props.dates.filter(d => d.date.month() == month)}
                        handleDateClick={(moment, dagsplan) => this.handleDateClick(moment, dagsplan)}
                        handleSelectDateHover={ date => this.handleSelectDateHover(date)}
                        key={"" + year + "-" + month}
                    />
                ))}
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
    };
}

const connectedDatePicker = connect(mapStateToProps)(DatePicker);
export { connectedDatePicker as DatePicker };