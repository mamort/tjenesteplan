import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components';
import Select from 'react-select';
import usersApi from '../Api/Users';
import { alertActions } from '../_actions';
import { breadcrumbActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';

class AddLegerToAvdeling extends React.Component {

  constructor(props) {
    super(props);

    this.handleLegeChange = this.handleLegeChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.initialize = this.initialize.bind(this);

    this.state = {
        legeOptions: [],
        selectedLegeId: null
    };
  }

  componentDidMount() {
    this.initialize();
  }

  initialize() {
    const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
    const avdelingId = parseInt(this.props.match.params.avdelingId, 10);

    this.props.initialize()
      .then(() => {
        const legeOptions = this.props.leger
            .filter(l => !l.avdelinger.includes(parseInt(avdelingId)))
            .map(l => {
          return {
            label: l.fullname,
            value: l.id
          }
        });

        const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
        const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
        this.props.initBreadcrumbs(sykehus, avdeling);

        this.setState({
          legeOptions,
          selectedSykehus: sykehus,
          selectedAvdeling: avdeling
        });
    });
  }

  handleLegeChange(legeId) {
    this.setState({
        selectedLegeId: legeId
    });
  }

  handleSubmit(event) {
    event.preventDefault();

    const avdelingId = parseInt(this.props.match.params.avdelingId, 10);

    const selectedLegeId = this.state.selectedLegeId;
    const lege = this.props.leger.find(l => l.id == selectedLegeId);

    if(selectedLegeId) {
        this.props.addLegeToAvdeling(avdelingId, selectedLegeId)
            .then(() => {
                this.initialize();
                this.setState({
                    selectedLegeId: null
                });


                this.props.displayMessage(lege.fullname + " lagt til avdeling " + this.state.selectedAvdeling.name);
            });

    }
  }

  render() {
    if(this.props.isFetching) {
      return <Loader />;
    }

    return (
      <div>
        <CenteredComponent>
          <div className="avdeling-leger">
            <h1>Leger</h1>

            <form name="form" onSubmit={this.handleSubmit}>
                <div className="form-group">
                <SelectLege
                    name="legeId"
                    style={{width: `100%`}}
                    selectedValue={this.state.selectedLegeId}
                    handleLegeChange={this.handleLegeChange}
                    options={this.state.legeOptions}
                />
                </div>

                <div className="form-group">
                    <button className="btn btn-primary">
                        Legg lege til avdeling
                    </button>
                </div>
            </form>

          </div>
        </CenteredComponent>
      </div>
    );
  }
}

function SelectLege(props) {
  const { name, options, selectedValue, handleLegeChange } = props;

  let selectedOption = null;

  if(selectedValue){
    selectedOption = options.find(o => o.value == selectedValue);
  }

  return (<Select
    name={name}
    placeholder="Velg lege"
    defaultValue={selectedValue}
    value={selectedOption}
    options={options}
    onChange={event => handleLegeChange(event.value)}
    hideSelectedOptions={true}
  />);
}

function mapStateToProps(state) {
    return {
      sykehus: state.sykehus.sykehus,
      isFetching: state.users.loading,
      leger: state.users.alleLeger
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: () => {
      return Promise.all([
        dispatch(sykehusApi.actions.getSykehus()),
        dispatch(usersApi.actions.getAlleLeger())
      ]);
    },

    initBreadcrumbs: (sykehus, avdeling) => {
      dispatch(
          breadcrumbActions.set(
            [
              { name: "Sykehus", link: "/sykehus"},
              { name: sykehus.name, link: "/sykehus/" + sykehus.id },
              { name: avdeling.name, link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id },
              { name: "Leger", link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id + '/leger' }
            ]
          )
      );
    },

    addLegeToAvdeling(avdelingId, legeId) {
      return dispatch(usersApi.actions.addLegeToAvdeling(avdelingId, legeId));
    },

    displayMessage: (msg) => {
        dispatch(alertActions.success(msg));
      }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(AddLegerToAvdeling);