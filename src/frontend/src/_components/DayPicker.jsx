import React from 'react';
import DayPickerInput from 'react-day-picker/DayPickerInput';

import MomentLocaleUtils, {
    formatDate,
    parseDate,
  } from 'react-day-picker/moment';

import 'moment/locale/nb';

class DayPicker extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const DATE_FORMAT = 'DD.MM.YYYY';

        return (
            <div className="daypicker">
                <DayPickerInput
                    onDayChange={this.props.onDayChange}
                    selectedDays={this.props.selectedDays}

                    formatDate={formatDate}
                    parseDate={parseDate}
                    format={DATE_FORMAT}
                    placeholder="dd.mm.åååå"
                    dayPickerProps={{
                        locale: 'nb',
                        localeUtils: MomentLocaleUtils,
                    }}
                />
            </div>
        );
    }
}

export { DayPicker };