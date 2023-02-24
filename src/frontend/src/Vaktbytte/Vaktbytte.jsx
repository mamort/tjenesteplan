import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment';
import 'moment/locale/nb';
import { DatePicker } from '../DatePicker/DatePicker.jsx';
import { dagsplanConstants } from '../_constants/dagsplan.constants';
import { vaktChangeRequestStatusConstants } from '../_constants/vaktChangeRequestStatus.constants';
import { vaktChangeRequestReplyStatusConstants } from '../_constants/vaktChangeRequestReplyStatus.constants';
import * as actions from './vaktbytte.actions';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { breadcrumbActions } from '../_actions';
import { alertActions } from '../_actions';

class Vaktbytte extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            selectStart: null,
            selectedDate: null,
            selectedDagsplan: null,
            dates: this.props.datoer,
            vaktbytteForslagDates: null,
            receivedRequest: null,
            receivedRequestMoment: null,
            receivedRequestDagsplan: null,
            isLoading: true
        };

        this.handleScroll = this.handleScroll.bind(this);
        this.handleIsDateSelectable = this.handleIsDateSelectable.bind(this);
        this.firstCalendarMonthRef = React.createRef();
    }

    componentDidMount() {
        this.setState({
            isLoading: true
        });

        window.addEventListener('scroll', this.handleScroll);

        const tjenesteplanId = parseInt(this.props.match.params.tjenesteplanId, 10);
        this.props.initialize(tjenesteplanId)
            .then(() => {
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

                const vaktChangeRequestId = parseInt(this.props.match.params.id, 10);
                const receivedRequest = this.props.receivedRequests.find(r => r.vaktChangeRequestId == vaktChangeRequestId);

                if(receivedRequest) {
                    const receivedRequestMoment = moment.utc(receivedRequest.date);
                    const receivedRequestDagsplan = this.props.dagsplaner.find(d => d.id == receivedRequest.dagsplan);

                    this.setState({
                        receivedRequest,
                        receivedRequestMoment,
                        receivedRequestDagsplan
                    })
                }

                this.setState({
                    isLoading: false
                });
            });
    }

    componentWillUnmount() {
        window.removeEventListener('scroll', this.handleScroll);
    }

    componentWillReceiveProps(newProps) {
        let newDates = newProps.datoer;
        if(newProps.isFetching === false && newProps.receivedRequests.length > 0){
            const vaktChangeRequestId = newProps.match.params.id;
            const receivedRequest = newProps.receivedRequests.find(r => r.vaktChangeRequestId == vaktChangeRequestId);
            if(receivedRequest) {
                let vaktbytteForslagDates = receivedRequest.reply.alternatives
                    .map(d => {
                        return {date: moment.utc(d), dagsplan: receivedRequest.dagsplan};
                        })
                    .filter(d => d.date.isAfter(moment.utc()));

                if(!this.state.vaktbytteForslagDates) {
                    this.setState({
                        vaktbytteForslagDates: vaktbytteForslagDates
                    });
                }else{
                    vaktbytteForslagDates = this.state.vaktbytteForslagDates;
                }

                newDates = newProps.datoer.map(d => {
                    if(vaktbytteForslagDates.find(ds => ds.date.isSame(d.date, 'day'))){
                        return {...d, isSelected: true};
                    }

                    return d;
                });
            }
        }

        this.setState({
            dates: newDates
        });
    }

    handleIsDateSelectable(date, dagsplan, isHoliday) {
        let selectableCategories = [];

        if(this.state.receivedRequestDagsplan) {
            selectableCategories = [
                this.state.receivedRequestDagsplan.id
            ];
        }

        if(dagsplan && !isHoliday) {
            const index = selectableCategories.indexOf(dagsplan.id);
            if(index > -1 && date.isAfter(moment.utc())) {
                return true;
            }
        }

        return false;
    }

    handleSelectStartDate(date) {
        const newDates = this.state.dates.map(d => {
            return d.date.isSame(date, 'day')
                ? {...d, isSelected: !d.isSelected}
                : d;
            });

        const selectedDates = newDates.filter(d => d.isSelected);
        const selectedDate = selectedDates.find(d => d.date.isSame(date, 'day'));
        const vaktbytteForslag = selectedDates.map(d => { return { date: d.date, dagsplan: d.dagsplan } });

        this.setState({
            dates: newDates,
            selectStart: null,
            selectedDate: selectedDate,
            selectedDagsplan: null,
            displayDagsplanPicker: false,
            vaktbytteForslagDates: vaktbytteForslag
        });
    }

    handleSelectEndDate(startDate, endDate, dagsplanId) {
        this.setState({
            selectStart: null,
            selectedDagsplan: null,
        });
    }

    handleSendInnForslag() {
        const vaktChangeRequestId = parseInt(this.props.match.params.id, 10);
        const receivedRequest = this.props.receivedRequests.find(r => r.vaktChangeRequestId == vaktChangeRequestId);

        this.props.saveVaktChangeSuggestions(
            receivedRequest.reply.id,
            this.state.vaktbytteForslagDates
        );
    }

    handleRejectVaktChangeRequest() {
        if (window.confirm("Er du sikker på at du ikke kan bytte vakt?")) {
            const vaktChangeRequestId = parseInt(this.props.match.params.id, 10);
            const receivedRequest = this.props.receivedRequests.find(r => r.vaktChangeRequestId == vaktChangeRequestId);
            this.props.rejectVaktChangeRequest(receivedRequest.reply.id)
                .then(() => {
                    this.setState({
                        vaktbytteForslagDates: []
                    });
                });
        }
    }

    handleScroll() {
        let isScrolledToCalendar = false;
        if(this.firstCalendarMonthRef.current) {
            const offset = this.firstCalendarMonthRef.current.offsetTop;
            var doc = document.documentElement;
            var top = (window.pageYOffset || doc.scrollTop)  - (doc.clientTop || 0);

            if(top > offset) {
                isScrolledToCalendar = true;
            }
        }

        if(isScrolledToCalendar != this.state.isScrolledToCalendar) {
            this.setState({
                isScrolledToCalendar
            });
        }
    }

    render() {
        if(this.props.isFetching || this.state.isLoading || this.props.receivedRequests.length === 0) {
            return (
                <Loader />
              )
        }

        const receivedRequest = this.state.receivedRequest;
        const receivedRequestMoment = this.state.receivedRequestMoment;
        const receivedRequestDagsplan = this.state.receivedRequestDagsplan;

        if(!receivedRequest) {
            return (
                <CenteredComponent><h2>Vaktbytte-forespørsel ble ikke funnet.</h2></CenteredComponent>
            );
        }

        if(receivedRequest.reply.status === vaktChangeRequestReplyStatusConstants.Rejected) {
            return (
                <CenteredComponent>
                    <h3>Du har avslått denne forespørselen om vaktbytte.</h3>
                </CenteredComponent>
            );
        }

        if(receivedRequest.status === vaktChangeRequestStatusConstants.Completed) {
            return (
                <CenteredComponent>
                    <h3>Vaktbytte er fullført</h3>
                </CenteredComponent>
            );
        }

        let sendButtonAttr = {};

        const vaktbytteForslagDates = this.state.vaktbytteForslagDates;
        if (!vaktbytteForslagDates || vaktbytteForslagDates.length === 0) {
            sendButtonAttr.disabled = 'disabled';
        }

        return (
            <div>
                { this.state.isScrolledToCalendar
                    ?
                        <div className="top-header vaktbytte-top-header">
                            <a href={"/leger/" + receivedRequest.requestedByUserId}>
                                {receivedRequest.requestedBy}
                            </a> ønsker å bytte {receivedRequestDagsplan.name.toLowerCase()} {receivedRequestMoment.format("dddd, Do MMMM")}
                        </div>
                    : null
                }

                <div
                    className="container datepicker vaktbytte bottom-header-spacing top-header-spacing"
                    onScroll={this.handleScroll}
                >
                    <h1>Foreslå vaktbytte</h1>
                    <div className="textbox">
                        <a href={"/leger/" + receivedRequest.requestedByUserId}>
                            {receivedRequest.requestedBy}
                        </a> med spesialisering {receivedRequest.RequestedBySpesialisering} ønsker å bytte {receivedRequestDagsplan.name.toLowerCase()}en sin <strong>{receivedRequestMoment.format("dddd, Do MMMM")}</strong>.
                        Velg de vaktene du kan bytte med han i kalenderen.
                    </div>
                    <div ref={this.firstCalendarMonthRef}></div>
                    <DatePicker
                        dates={this.state.dates}
                        dagsplaner={this.props.dagsplaner}
                        isDateSelectable={this.handleIsDateSelectable}
                        selectStart={this.state.selectStart}
                        selectedDagsplan={this.state.selectedDagsplan}
                        onSelectStartDate={moment => this.handleSelectStartDate(moment)}
                        onSelectEndDate={(startdate, enddate, selectedDagsplan) => this.handleSelectEndDate(startdate, enddate, selectedDagsplan)}
                    />
                </div>
                <div className="bottom-header vaktbytte-bottom-header">
                    <SelectedDay
                        dagsplaner={this.props.dagsplaner}
                        selectedDate={this.state.selectedDate}
                    />
                    <div className="vaktbytte-bottom-header-buttons">
                        { !this.props.isSaving
                            ?
                                <div>
                                    <button
                                        type="button"
                                        className="btn btn-danger"
                                        onClick={() => this.handleRejectVaktChangeRequest()}
                                    >
                                            Avslå vaktbytte forespørsel
                                    </button>
                                    <button
                                        type="button"
                                        className="btn btn-primary"
                                        onClick={() => this.handleSendInnForslag()}
                                        {...sendButtonAttr}
                                    >
                                            Send inn forslag til vaktbytte
                                    </button>
                                </div>
                            :
                                <SaveLoader isSaving={this.props.isSaving}/>
                        }


                    </div>
                </div>
            </div>
        );
    }
}

function SelectedDay(props) {
    const { dagsplaner, selectedDate } = props;

    if(!selectedDate) {
        return null;
    }

    const selectedDagsplan = dagsplaner.find(d => d.id == selectedDate.dagsplanId);

    return (
        <div className="vaktbytte-selectedDay">
            {selectedDate.date.format("MMM Do")} - {selectedDagsplan.name}
        </div>
    )
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

function mapStateToProps(state) {
    return {
        dagsplaner: state.dagsplaner.alleDagsplaner,
        sykehus: state.sykehus.sykehus,
        sykehusId: state.mintjenesteplan.sykehusId,
        avdelingId: state.mintjenesteplan.avdelingId,
        tjenesteplanName: state.mintjenesteplan.name,
        datoer: state.mintjenesteplan.datoer,
        receivedRequests: state.vaktChangeRequests.received.requests,
        isFetching: state.vaktChangeRequests.isFetching ||
                    state.vaktChangeRequests.received.isFetching ||
                    state.mintjenesteplan.isFetching ||
                    state.dagsplaner.isFetching,

        isSaving: state.vaktChangeRequests.suggestions.isSaving
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      initialize: (tjenesteplanId) => {
        return dispatch(actions.initialize(tjenesteplanId))
      },

      initBreadcrumbs: (sykehus, avdeling, tjenesteplan) => {
        dispatch(
            breadcrumbActions.set(
              [
                { name: sykehus.name },
                { name: avdeling.name },
                { name: tjenesteplan.name, link: '/minetjenesteplaner/' + tjenesteplan.id }
              ]
            )
        );
    },

      saveVaktChangeSuggestions: (id, dates) => {
        dispatch(actions.saveVaktChangeSuggestions(id, dates))
        .then(() => {
            dispatch(alertActions.success('Forslag til vaktbytte registrert'));
        })
        .catch(() => {
            dispatch(alertActions.error('Kunne ikke registrere forslag til vaktbytte pga en feil'));
        });
      },

      rejectVaktChangeRequest: (id) => {
        return dispatch(actions.saveVaktChangeSuggestions(id, []))
            .then(() => {
                dispatch(alertActions.success('Vaktbytte avslått'));
            })
            .catch(() => {
                dispatch(alertActions.error('Kunne ikke avslå vaktbytte pga en feil'))
            });
      }
    }
  }

const VaktbytteConnected = connect(mapStateToProps, mapDispatchToProps)(Vaktbytte);
export { VaktbytteConnected as Vaktbytte };