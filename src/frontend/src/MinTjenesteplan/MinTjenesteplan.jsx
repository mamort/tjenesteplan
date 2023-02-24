import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import { DatePicker } from '../DatePicker/DatePicker.jsx';
import { alertActions } from '../_actions';
import { ByttVakt } from './ByttVakt.jsx';
import { VakansvaktDialog } from './VakansvaktDialog.jsx';
import { dagsplanConstants } from '../_constants/dagsplan.constants'
import * as actions from './minTjenesteplan.actions';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { breadcrumbActions } from '../_actions';
import scroll from '../_helpers/scroll';

class Lege extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            displayMyDays: false,
            displayVaktDialog: false,
            displayVakansvaktDialog: false,
            lastSelectedDate: {
                date: null,
                dagsplan: null
            },
            isSelectRangeActive: false,
            dagsplaner: [],
            dagsplanerStatistics: [],
            dates: []
        };

        this.closeVaktDialog = this.closeVaktDialog.bind(this);
        this.openVaktDialog = this.openVaktDialog.bind(this);
        this.closeVakansvaktDialog = this.closeVakansvaktDialog.bind(this);
        this.openVakansvaktDialog = this.openVakansvaktDialog.bind(this);
    }

    componentDidMount() {
        const tjenesteplanId = parseInt(this.props.match.params.id, 10);
        this.props.initialize(tjenesteplanId)
            .then(() => {
                const sykehus = this.props.sykehus.find(s => s.id == this.props.sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == this.props.avdelingId);
                this.props.initBreadcrumbs(sykehus, avdeling);
            });
    }

    componentWillReceiveProps(newProps) {
        let newDates = newProps.datoer;

        if(newDates && newDates !== this.props.datoer) {
            const sortedDates = newDates.sort(function(a,b){
                return a.date.isAfter(b.date, 'day');
            });

            this.setState({
                earliestDate: sortedDates[0],
                dates: newDates
            });
        }
    }

    componentDidUpdate(prevProps, prevState) {
        if(prevState.dates !== this.state.dates || prevProps.dagsplaner !== this.props.dagsplaner) {
            var dagsplanerStatistics = this.props.dagsplaner.map(dp => {
                var count = this.state.dates.filter(d => d.dagsplanId === dp.id).length;
                return {
                    id: dp.id,
                    name: dp.name,
                    count: count
                };
            });

            this.setState({
                dagsplanerStatistics: dagsplanerStatistics.filter(ds => ds.count > 0)
            });
        }
    }

    cancelVaktDialog() {
        this.deselectAllDates();
        this.setState({
            displayVaktDialog: false
        });
    }

    cancelVakansvaktDialog() {
        this.deselectAllDates();
        this.setState({
            displayVakansvaktDialog: false
        });
    }

    deselectAllDates() {
        const newDates = this.state.dates.map(d => {
                return !d.isSelected ? d : {...d, isSelected: false};
            }
        );

        this.setState({
            dates: newDates,
            lastSelectedDate: {
                date: null,
                dagsplan: null
            }
        });
    }

    openVaktDialog(evt) {
        evt.preventDefault();

        this.setState({
            displayVaktDialog: true
        });
    }

    openVakansvaktDialog(evt) {
        evt.preventDefault();

        this.setState({
            displayVakansvaktDialog: true
        });
    }

    closeVaktDialog() {
        this.setState({
            displayVaktDialog: false
        });
    }

    closeVakansvaktDialog() {
        this.setState({
            displayVakansvaktDialog: false
        });
    }

    handleIsDateSelectable(date, dagsplan, isHoliday) {
        if(isHoliday) {
            return true;
        }

        if(dagsplan && dagsplan.id > 0) {
            return true;
        }

        if(!this.state.earliestDate || date.isSameOrAfter(this.state.earliestDate.date, 'day')) {
            return true;
        }

        return false;
    }

    handleSelectStartDate(date, dagsplan) {
        const newDates = this.state.dates.map(d => {
            if (d.date.isSame(date, 'day') ||
                (this.state.lastSelectedDate.date && this.state.isSelectRangeActive &&
                this.state.lastSelectedDate.date.isBefore(date) &&
                d.date.isSameOrAfter(this.state.lastSelectedDate.date, 'day') &&
                d.date.isSameOrBefore(date, 'day'))) {

                    // Avoid mutating state if it is same value
                    return d.isSelected ? d : {...d, isSelected: true};

                } else {
                    // Avoid mutating state if it is same value
                    return !d.isSelected ? d : {...d, isSelected: false};
                }
            }
        );

        let isRangeSelected = false;
        const selectedDates = newDates.filter(d => d.isSelected);
        if(selectedDates.length > 0) {
            isRangeSelected = true;
        }

        this.setState({
            isSelectRangeActive: this.isSelectRangeActive(this.state, date),
            isRangeSelected: isRangeSelected,
            lastSelectedDate: this.getLastSelectedDate(
                this.props.dagsplaner,
                this.state.dates,
                date
            ),
            displayVaktDialog: false,
            dates: newDates,
            selectedDates: selectedDates
        });
    }

    handleSelectEndDate(startDate, endDate, dagsplanId) {
        if(endDate.isSameOrAfter(startDate) && this.state.isSelectRangeActive) {
            const newDates = this.state.dates.map(d => {
                if(d.date.isSameOrAfter(startDate, 'day') && d.date.isSameOrBefore(endDate, 'day'))
                {
                    return d.isSelected ? d : {...d, isSelected: true};
                } else {
                    return !d.isSelected ? d : {...d, isSelected: false};
                }
            });

            this.setState({
                isRangeSelected: false,
                selectedDates: null,
                isSelectRangeActive: this.isSelectRangeActive(this.state, endDate),
                dates: newDates
            });
        } else {
            let newDates = this.state.dates;

            if(!this.state.isSelectRangeActive) {
                newDates = this.state.dates.map(d => {
                    if(d.date.isSame(endDate, 'day'))
                    {
                        return d.isSelected ? d : {...d, isSelected: true};
                    } else {
                        return !d.isSelected ? d : {...d, isSelected: false};
                    }
                });
            }

            this.setState({
                isSelectRangeActive: this.isSelectRangeActive(this.state, endDate),
                isRangeSelected: false,
                dates: newDates,
                lastSelectedDate: this.getLastSelectedDate(
                    this.props.dagsplaner,
                    this.state.dates,
                    endDate
                ),
                selectedDates: null
            });
        }
    }

    getLastSelectedDate(dagsplaner, dates, date) {
        const plandate = dates.find(d => d.date.isSame(date, 'day'));
        let dagsplan = null;
        if(plandate && plandate.dagsplanId) {
            dagsplan = dagsplaner.find(d => d.id == plandate.dagsplanId);
        }

        let description = "";
        if(plandate) {

            if(plandate.dagsplanId > 0 && plandate.description) {
                description = plandate.description + " - " + dagsplan.name;
            } else if(plandate.dagsplanId > 0) {
                description = dagsplan.name;
            } else if(plandate.description) {
                description = plandate.description
            }
        }

        return {
            date: date,
            dagsplan: dagsplan,
            description: description
        };
    }

    isSelectRangeActive(state, date) {
        // Disable select date ranges until we need this functionality
        return false;

        let isSelectRangeActive = state.isSelectRangeActive;
        if(state.lastSelectedDate.date != null && date.isSame(state.lastSelectedDate.date, 'day')){
            isSelectRangeActive = !isSelectRangeActive;
        }

        return isSelectRangeActive;
    }

    shouldDisplayVaktActions() {
        if(this.state.lastSelectedDate.dagsplan) {
            // Is before todays date
            if(this.state.lastSelectedDate.date.isBefore(new Date(), "day")){
                return false;
            }

            const dagsplanId = this.state.lastSelectedDate.dagsplan.id;
            return  dagsplanId === dagsplanConstants.Døgnvakt ||
                    dagsplanId === dagsplanConstants.Nattevakt ||
                    dagsplanId === dagsplanConstants.Dagvakt;
        }

        return false;
    }

    shouldDisplayVaktRequestActions() {
        if(this.state.lastSelectedDate.dagsplan) {
            const dagsplanId = this.state.lastSelectedDate.dagsplan.id;
            return dagsplanId === dagsplanConstants.ForespørselOmVaktbytte;
        }

        return false;
    }

    handleByttVakt() {
        const tjenesteplanId = parseInt(this.props.match.params.id, 10);
        const selectedDate = this.state.lastSelectedDate;

        var topScrollPosition = scroll.getTopScrollPosition();

        this.props.byttVakt(
            tjenesteplanId,
            selectedDate.dagsplan.id,
            selectedDate.date
        ).then(() => {
            window.scrollTo({ top: topScrollPosition });
        });

        this.setState({
            displayVaktDialog: false
        });

        this.deselectAllDates();
    }

    undoVaktbytte(evt) {
        evt.preventDefault();

        const tjenesteplanId = parseInt(this.props.match.params.id, 10);
        const selectedDate = this.state.lastSelectedDate;
        const request = this.props.vaktChangeRequests.find(r => moment.utc(r.date).isSame(selectedDate.date, 'day'));

        var topScrollPosition = scroll.getTopScrollPosition();
        this.props.undoVaktbytte(tjenesteplanId, request.id, selectedDate.date)
        .then(() => {
            window.scrollTo({ top: topScrollPosition });
        });

        this.deselectAllDates();
    }

    sendVakansvaktRequest(newDagsplanId, reason) {
        const tjenesteplanId = parseInt(this.props.match.params.id, 10);
        const lastSelectedDate = this.state.lastSelectedDate;
        var topScrollPosition = scroll.getTopScrollPosition();

        this.props.sendVakansvaktRequest(
            tjenesteplanId,
            lastSelectedDate.date,
            newDagsplanId,
            reason
        ).then(() => {
            window.scrollTo({ top: topScrollPosition });
        });

        this.setState({
            displayVakansvaktDialog: false
        });

        this.deselectAllDates();
    }

    toggleMyDays() {
        const displayMyDays = !this.state.displayMyDays;

        if(displayMyDays) {
            this.deselectAllDates();
            this.setState({
                displayMyDays
            });
        }else{
            this.setState({
                displayMyDays
            });
        }
    }

    render() {
        if(this.props.isFetching && this.state.dates.length === 0) {
            return <Loader />;
        }

        if(!this.props.isTjenesteplanAssigned) {
            return (
                <div className="notfound-message">
                    <CenteredComponent  className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                        <div className="alert alert-secondary" role="alert">
                            Du har ikke fått tildelt en tjenesteplan.
                        </div>
                    </CenteredComponent>
                </div>
            );
        }

        const displayVaktActions = this.shouldDisplayVaktActions();
        const displayVaktRequestActions = this.shouldDisplayVaktRequestActions();

        return (
            <div className={this.state.isSelectRangeActive
                ? "minTjenesteplan minTjenesteplan--selectRangeActive"
                : "minTjenesteplan"
            }>
                <div className="container datepicker">
                    <h1>Tjenesteplan {this.props.name}</h1>
                    <ByttVakt
                        date={this.state.lastSelectedDate.date}
                        isOpen={this.state.displayVaktDialog}
                        onClose={() => this.closeVaktDialog() }
                        onCancel={() => this.cancelVaktDialog()}
                        dagsplaner={this.props.dagsplaner}
                        onByttVakt={ () => this.handleByttVakt()}
                    />

                    <VakansvaktDialog
                        date={this.state.lastSelectedDate.date}
                        isOpen={this.state.displayVakansvaktDialog}
                        onClose={() => this.closeVakansvaktDialog() }
                        onCancel={() => this.cancelVakansvaktDialog()}
                        onSubmit={ (newDagsplanId, reason) => this.sendVakansvaktRequest(newDagsplanId, reason)}
                    />

                    <DatePicker
                        dates={this.state.dates}
                        dagsplaner={this.props.dagsplaner}
                        isDateSelectable={(date, dagsplan, isHoliday) => this.handleIsDateSelectable(date, dagsplan, isHoliday)}
                        selectStart={this.state.lastSelectedDate.date}
                        selectedDagsplan={this.state.lastSelectedDate.dagsplan}
                        onSelectStartDate={(moment, dagsplan) => this.handleSelectStartDate(moment, dagsplan)}
                        onSelectEndDate={(startdate, enddate, selectedDagsplanId) => this.handleSelectEndDate(startdate, enddate, selectedDagsplanId)}
                    />
                </div>
                <div className="mintjenesteplan-footer-menu"
                    className={
                        this.state.displayMyDays
                            ? "mintjenesteplan-footer-menu mintjenesteplan-footer-menu--showitems"
                            : "mintjenesteplan-footer-menu"
                        }
                >
                    <div className="mintjenesteplan-footer-menu-spacer"></div>
                    <ul className={
                        this.state.displayMyDays
                            ? "mintjenesteplan-footer-items mintjenesteplan-footer-items--show"
                            : "mintjenesteplan-footer-items"
                        }
                    >
                        {this.state.dagsplanerStatistics.map(d => {
                            return (
                                <li key={d.id}><i className={"datepickerDay--dagsplan-" + d.id} />{d.name} ({d.count})</li>
                            )
                        })}
                    </ul>

                    <ul className={
                        displayVaktActions
                            ? "mintjenesteplan-footer-actionitems mintjenesteplan-footer-actionitems--show"
                            : "mintjenesteplan-footer-actionitems"
                        }
                    >
                        <li><a href="#" onClick={(evt) => this.openVaktDialog(evt)}><i className="dripicons-swap"/>Bytt vakt</a></li>
                        <li><a href="#" onClick={(evt) => this.openVakansvaktDialog(evt)}><i className="dripicons-skip"/>Send vakansvakt forespørsel</a></li>
                    </ul>

                    <ul className={
                        displayVaktRequestActions
                            ? "mintjenesteplan-footer-actionitems mintjenesteplan-footer-actionitems--show"
                            : "mintjenesteplan-footer-actionitems"
                        }
                    >
                        <li><a href="#" onClick={(evt) => this.undoVaktbytte(evt)}><i className="dripicons-cross"/>Slett forespørsel</a></li>
                    </ul>


                    <div className="mintjenesteplan-footer-menu-header">
                        <div className="mintjenesteplan-footer-minedager"
                            onClick={() => this.toggleMyDays()}
                        >
                            <i className={
                                    this.state.displayMyDays
                                    ? "dripicons-cross"
                                    : "dripicons-view-thumb"
                                }
                            ></i>
                            <span>Mine dager</span>
                        </div>
                        <div className="mintjenesteplan-footer-edit">
                            <ElementDescription
                                lastSelectedDate={this.state.lastSelectedDate}
                                selectedDates={this.state.selectedDates}
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

function ElementDescription(props) {
    const { lastSelectedDate, selectedDates } = props;

    if(selectedDates && selectedDates.length > 1) {
        const sortedDates = selectedDates.sort(function(a,b){
            return a.date.isAfter(b.date, 'day');
        });

        const firstDate = sortedDates[0];
        const lastDate = sortedDates[sortedDates.length-1];

        return (<span>{firstDate.date.format("MMM Do")} - {lastDate.date.format("MMM Do")}</span>)
    }

    if(lastSelectedDate.date) {
        let description = lastSelectedDate.date.format("Do MMM");
        if(lastSelectedDate.description) {
            description += " - " + lastSelectedDate.description
        }
        return (<span>{description}</span>)
    }

    return null;
}

function mapStateToProps(state) {
    return {
        dagsplaner: state.dagsplaner.alleDagsplaner,
        sykehus: state.sykehus.sykehus,
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        name: state.mintjenesteplan.name,
        datoer: state.mintjenesteplan.datoer,
        vaktChangeRequests: state.vaktChangeRequests.requests,
        isTjenesteplanAssigned: state.mintjenesteplan.isAssigned,
        isFetching: state.mintjenesteplan.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      initialize: (tjenesteplanId) => {
        return dispatch(actions.initialize(tjenesteplanId))
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

      byttVakt: (tjenesteplanId, dagsplan, date) => {
        return dispatch(actions.saveVaktbytter(tjenesteplanId, dagsplan, [date]))
            .then(() => {
                dispatch(alertActions.success('Forespørsel om vaktbytte ' + date.format("Do MMM") + ' registrert'));
                return dispatch(actions.initialize(tjenesteplanId));
            }).catch(() => {
                dispatch(alertActions.error('Beklager, fikk ikke registrert forespørsel pga en feil'));
            })
      },

      undoVaktbytte: (tjenesteplanId, vaktChangeRequestId, date) => {
        return dispatch(actions.undoVaktbytte(tjenesteplanId, vaktChangeRequestId))
            .then(() => {
                dispatch(alertActions.success('Forespørsel om vaktbytte ' + date.format("Do MMM") + ' slettet'));
                return dispatch(actions.initialize(tjenesteplanId))
            }).catch(() => {
                dispatch(alertActions.error('Beklager, kunne ikke slette forespørsel om vaktbytte pga en feil'));
            })
      },

      sendVakansvaktRequest: (tjenesteplanId, date, newDagsplanId, reason) => {
        return dispatch(actions.sendVakansvaktRequest(tjenesteplanId, date, newDagsplanId, reason))
            .then(() => dispatch(actions.initialize(tjenesteplanId)));
      }

    }
  }

export default connect(mapStateToProps, mapDispatchToProps)(Lege);