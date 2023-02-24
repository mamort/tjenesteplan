import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import Select from 'react-select';
import { CenteredComponent } from '../_components';
import { Loader } from '../_components/Loader.jsx';
import { alertActions } from '../_actions';
import usersApi from '../Api/Users';
import sykehusApi from '../Api/Sykehus';
import avdelingerApi from '../Api/Avdelinger';
import { breadcrumbActions } from '../_actions';

class AssignListeforer extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            selectedSykehus: null,
            selectedAvdeling: null
        };

        this.handleListeforerChange = this.handleListeforerChange.bind(this);
        this.handleAssignSubmit = this.handleAssignSubmit.bind(this);
      }

    componentDidMount() {
        const self = this;
        const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
        const avdelingId = parseInt(this.props.match.params.id, 10);

        this.props.initialize()
            .then(() => {
                if(sykehusId) {
                    const sykehus  = self.props.sykehus.find(s => s.id == sykehusId);
                    const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);

                    let listeforer = null;
                    if(avdeling.listeforerId) {
                        listeforer = this.props.leger.find(l => l.id == avdeling.listeforerId);
                    }

                    this.props.initBreadcrumbs(sykehus, avdeling);

                    self.setState({
                        selectedSykehus: sykehus,
                        selectedAvdeling: avdeling,
                        selectedListeforer: listeforer
                    });
                }
            });
    }

    handleListeforerChange(legeId) {
        const listeforer = this.props.leger.find(l => l.id == legeId);
        this.setState({
            selectedListeforer: listeforer
        });
    }

    handleAssignSubmit(event) {
        event.preventDefault();

        const selectedAvdeling = this.state.selectedAvdeling;
        const selectedListeforer = this.state.selectedListeforer;

        if(this.state.selectedListeforer) {
            this.props.assignListeforerToAvdeling(
                selectedAvdeling,
                selectedListeforer.id,
            ).then(() => {
                this.props.displayMessage(
                    selectedListeforer.fullname + " er nå listefører for avdeling " + selectedAvdeling.name
                );
            });
        }
    }

    render() {
        const sykehus = this.state.selectedSykehus;
        const avdeling = this.state.selectedAvdeling;

        if(!sykehus) {
            return <Loader />;
        }

        let options = this.props.leger.map(lege => ({ value: lege.id, label: lege.fullname }));
        let selectedValue = { value: -1, label: "Velg lege" };

        const listeforer = this.state.selectedListeforer;
        if(listeforer){
            selectedValue = { value: listeforer.id, label: listeforer.fullname };
        }else{
            options.push(selectedValue);
        }

        return (
            <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                <div className="assignListeforer">
                    <h1>{sykehus.name} - {avdeling.name}</h1>
                    <br /><br />
                    <h2>Tildel listefører</h2>
                    <form name="form" onSubmit={this.handleAssignSubmit}>
                    <div className="form-group">
                    <Select
                        name="selectedUserId"
                        className=""
                        defaultValue={selectedValue}
                        value={selectedValue}
                        options={options}
                        onChange={event => this.handleListeforerChange(event.value)}
                        hideSelectedOptions={true}
                    />
                    </div>

                    <div className="form-group">
                        <button className="btn btn-primary">
                            Sett lege som listefører
                        </button>
                    </div>
                    </form>
                </div>
            </CenteredComponent>
        );
    }
}

function mapStateToProps(state) {
    return {
        sykehus: state.sykehus.sykehus,
        leger: state.users.leger
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      initialize: () => {
        return Promise.all([
            dispatch(usersApi.actions.getLeger()),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
      },

      initBreadcrumbs: (sykehus, avdeling) => {
        dispatch(
            breadcrumbActions.set(
                [
                    { name: "Sykehus", link: "/sykehus"},
                    { name: sykehus.name, link: "/sykehus/" + sykehus.id },
                    { name: avdeling.name, link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id }
                ]
            )
        );
    },

      assignListeforerToAvdeling: (avdeling, listeforerId) => {
        return dispatch(avdelingerApi.actions.updateAvdeling(avdeling.id, avdeling.name, listeforerId))
      },

      displayMessage: (msg) => {
        dispatch(alertActions.success(msg));
      }
    }
  }

const connectedAssignListeforer = connect(mapStateToProps, mapDispatchToProps)(AssignListeforer);
export { connectedAssignListeforer as AssignListeforer };