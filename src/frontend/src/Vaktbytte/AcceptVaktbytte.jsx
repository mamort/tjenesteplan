import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import { DatePicker } from '../DatePicker/DatePicker.jsx';
import { dagsplanConstants } from '../_constants/dagsplan.constants';
import { vaktChangeRequestStatusConstants } from '../_constants/vaktChangeRequestStatus.constants'
import * as actions from './vaktbytte.actions';
import { Loader } from '../_components/Loader.jsx';
import { breadcrumbActions } from '../_actions';
import { alertActions } from '../_actions';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';

class AcceptVaktbytte extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            selectStart: null,
            dates: this.props.datoer,
            acceptedDate: null
        };
    }

    componentDidMount() {
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
        this.props.initialize(tjenesteplanId)
            .then(() => {
                if(this.props.isTjenesteplanAssigned) {
                    const sykehus = this.props.sykehus.find(s => s.id == this.props.sykehusId);
                    const avdeling = sykehus.avdelinger.find(a => a.id == this.props.avdelingId);
                    this.props.initBreadcrumbs(
                        sykehus,
                        avdeling,
                        {
                            id: tjenesteplanId,
                            name: this.props.tjenesteplanName
                        }
                    );
                }
            });
    }

    componentWillReceiveProps(newProps) {
        let newDates = newProps.datoer;
        if(newProps.isFetching === false && newProps.requests.length > 0){
            const vaktChangeRequestId = newProps.match.params.id;
            const request = newProps.requests.find(r => r.id == vaktChangeRequestId);

            const selectableDates = [];
            for(let i = 0; i < request.replies.length; i++) {
                const reply = request.replies[i];

                for(let k = 0; k < reply.alternatives.length; k++){
                    const alternativeDate = reply.alternatives[k];
                    selectableDates.push({
                        replyId: reply.id,
                        date: moment.utc(alternativeDate),
                        dagsplanId: dagsplanConstants.None,
                        isSelected: false,
                        isVaktbytteSuggestion: true
                    });
                }
            }

            const acceptedDate = this.state.acceptedDate;
            newDates = newProps.datoer.map(d => {
                if(acceptedDate && this.isSameDate(d.date, acceptedDate.date)){
                    return {...acceptedDate, isSelected: true};
                }

                const selectableDate = selectableDates.find(ds => this.isSameDate(ds.date, d.date));
                if(selectableDate){
                    return { ...selectableDate, ...d };
                }

                return d;
            });

            for(let i = 0; i < selectableDates.length; i++) {
                const selectableDate = selectableDates[i];
                const existingDate = newDates.find(d => this.isSameDate(d.date, selectableDate.date));
                if(!existingDate){
                    newDates.push(selectableDate);
                } else {
                    newDates = newDates.filter(d => !this.isSameDate(d.date, selectableDate.date));
                    newDates.push({ ...existingDate, isVaktbytteSuggestion: true });
                }
            }

        }

        this.setState({
            dates: newDates
        });
    }

    isSameDate(d1, d2){
        return d1.year() === d2.year() &&
        d1.month() === d2.month() &&
        d1.date() === d2.date()
    }

    handleIsDateSelectable(date, dagsplan, isHoliday, dagsplanDay) {
        if(!dagsplanDay) return false;

        return dagsplanDay.isVaktbytteSuggestion || false;
    }

    handleIsDateMarked(dagsplanDay) {
        if(!dagsplanDay) return false;

        return dagsplanDay.isVaktbytteSuggestion || false;
    }

    handleSelectStartDate(date) {
        const newDates = this.state.dates.map(d => {
            return d.date.isSame(date, 'day')
                ? {...d, isSelected: !d.isSelected}
                : {...d, isSelected: false};
            }
        );

        const selectedDate = newDates.find(d => d.isSelected);

        this.setState({
            dates: newDates,
            selectStart: null,
            acceptedDate: selectedDate
        });
    }

    handleSelectEndDate(startDate, endDate, dagsplanId) {
        this.setState({
            selectStart: null
        });
    }

    handleAcceptVaktbytte() {
        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);

        this.props.acceptVaktbytte(
            tjenesteplanId,
            this.state.acceptedDate.replyId,
            this.state.acceptedDate.date
        );
    }

    handleAbort() {
        this.setState({
            acceptedDate: null
        });

        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
        this.props.initialize(tjenesteplanId);
    }

    render() {
        if(this.props.isFetching) {
            return (
                <Loader />
              )
        }

        const tjenesteplanId = this.props.match.params.tjenesteplanId;
        const vaktChangeRequestId = this.props.match.params.id;

        if(!this.props.isTjenesteplanAssigned) {
            return <CenteredComponent><h2>Fant ikke tjenesteplanen med id {tjenesteplanId}</h2></CenteredComponent>
        }

        if(this.props.requests.length === 0) {
            return <CenteredComponent><h2>Fant ikke vaktbytte med id {vaktChangeRequestId}</h2></CenteredComponent>
        }

        const request = this.props.requests.find(r => r.id == vaktChangeRequestId);
        const date = moment.utc(request.date);

        if(request.status === vaktChangeRequestStatusConstants.Completed) {
            return (
                <div className="absolute-position fill-height fill-width">
                    <div className="container fill-height fill-width vaktbytte">
                        <div className="row align-items-center justify-content-center fill-height fill-width">\
                            <div>
                                <h3>Vaktbytte er utført</h3>
                                <Link to={"/MineTjenesteplaner/" + tjenesteplanId}>Tilbake til Tjenesteplan</Link>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }

        let sendButtonAttr = {};

        if (!this.state.acceptedDate) {
            sendButtonAttr.disabled = 'disabled';
        }

        let avbrytButton;

        if (this.state.acceptedDate) {
            avbrytButton = <button type="button" className="btn btn-secondary" onClick={() => this.handleAbort()}>Avbryt</button>
        }

        return (
            <div>
                <div className="container datepicker vaktbytte bottom-header-spacing">
                    <h1>Aksepter vaktbytte</h1>
                    <div className="textbox">
                        Bytte av vakt <strong>{date.format("dddd, Do MMMM")}</strong>.
                        Velg den datoen du vil bytte til
                    </div>
                    <DatePicker
                        dates={this.state.dates}
                        dagsplaner={this.props.dagsplaner}
                        isDateSelectable={this.handleIsDateSelectable}
                        isDateMarked={this.handleIsDateMarked}
                        selectStart={this.state.selectStart}
                        selectedDagsplan={this.state.selectedDagsplan}
                        onSelectStartDate={moment => this.handleSelectStartDate(moment)}
                        onSelectEndDate={(startdate, enddate, selectedDagsplan) => this.handleSelectEndDate(startdate, enddate, selectedDagsplan)}
                    />
                </div>
                <div className="bottom-header">
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={() => this.handleAcceptVaktbytte()}
                        {...sendButtonAttr}
                    >
                            Aksepter vaktbytte
                    </button>
                    {avbrytButton}
                </div>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        dagsplaner: state.dagsplaner.alleDagsplaner,
        sykehus: state.sykehus.sykehus,
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        isTjenesteplanAssigned: state.mintjenesteplan.isAssigned,
        tjenesteplanName: state.mintjenesteplan.name,
        datoer: state.mintjenesteplan.datoer,
        requests: state.vaktChangeRequests.requests,
        isFetching: state.vaktChangeRequests.isFetching ||
                    state.mintjenesteplan.isFetching ||
                    state.dagsplaner.isFetching
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      initialize: (tjenesteplanId) => {
        return dispatch(actions.initializeAcceptVaktbytter(tjenesteplanId))
      },

      initBreadcrumbs: (sykehus, avdeling, tjenesteplan) => {
        dispatch(
            breadcrumbActions.set(
                [{ name: sykehus.name },
                 { name: avdeling.name },
                 { name: tjenesteplan.name, link: '/minetjenesteplaner/' + tjenesteplan.id }
                ]
            )
        )
    },

      acceptVaktbytte: (tjenesteplanId, id, date) => {
        dispatch(actions.acceptVaktbytte(tjenesteplanId, id, date))
        .catch(() => {
            dispatch(alertActions.error('Kunne ikke akseptere vaktbytte pga en feil'));
        });
      }
    }
  }

const AcceptVaktbytteConnected = connect(mapStateToProps, mapDispatchToProps)(AcceptVaktbytte);
export { AcceptVaktbytteConnected as AcceptVaktbytte };