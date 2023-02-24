import React from 'react';
import { connect } from 'react-redux';
import * as actions from './tjenesteplanLeger.actions';
import tjenesteplaner from '../../Api/Tjenesteplaner';
import { Loader } from '../../_components/Loader.jsx';

function CenterContainerComponent(props) {
  return (
    <div className="absolute-position fill-height fill-width">
      <div className="container fill-height fill-width">
        <div className="row align-items-center justify-content-center fill-height fill-width">
          {props.children}
        </div>
      </div>
    </div>
  );
}

function RegistrerLegeForm(props) {
  const { isFull, legeId, leger, handleRegistrationChange, handleRegistrationSubmit } = props;
  if(leger.length === 0) {
    return (
      <span>Finnes ingen flere leger å registrere.</span>
    );
  }

  if(isFull) {
    return (
      <span>Kan ikke registrere flere leger i denne tjenesteplanen.</span>
    );
  }

  return (
    <form name="form" onSubmit={handleRegistrationSubmit}>
      <div className="form-group">
        <label htmlFor="name">Navn</label>
        <select
          className="form-control"
          name="legeId"
          value={legeId}
          onChange={handleRegistrationChange}
        >
          {leger.map((lege, index) =>
              (<option value={lege.id} key={index}>{lege.fullName}</option>)
          )}
        </select>
      </div>
      <div className="form-group">
        <button className="btn btn-primary">Registrer lege</button>
      </div>
    </form>
  );
}

function LegerForm(props) {
  const { leger, handleRemoveLege } = props;

  if(leger.length === 0) {
    return (
      <span>Ingen leger i tjenesteplanen</span>
    );
  }

  return (
    <table className="table">
    <thead>
      <th scope="col">#</th>
      <th scope="col">Fornavn</th>
      <th scope="col">Etternavn</th>
      <th scope="col"></th>
    </thead>
    <tbody>
      {leger.map((lege, index) => (
          <tr key={index}>
            <td>{index}</td>
            <td>{lege.firstname}</td>
            <td>{lege.lastname}</td>
            <td>
              <button className="btn btn-link" onClick={() => handleRemoveLege(lege.id)}>Fjern</button>
            </td>
          </tr>
      ))}
  </tbody>
  </table>
  );
}

class TjenesteplanLeger extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      registration: {
        legeId: -1
      }
    };

    this.handleRegistrationChange = this.handleRegistrationChange.bind(this);
    this.handleRegistrationSubmit = this.handleRegistrationSubmit.bind(this);
    this.handleRemoveLege = this.handleRemoveLege.bind(this);
  }

  componentDidMount() {
    const id = parseInt(this.props.match.params.id, 10);
    this.props.initialize(id);
  }

  componentWillReceiveProps(newProps) {
    if(newProps.leger.length > 0 &&
      this.state.registration.legeId < 0) {
      this.setState({
        registration: {
          legeId: newProps.leger[0].id
        }
      });
    }
  }

  handleRegistrationChange(event) {
    const { name, value } = event.target;
    const { registration } = this.state;
    this.setState({
      registration: {
            ...registration,
            [name]: value
        }
    });
  }

  handleRegistrationSubmit(event) {
      event.preventDefault();

      const tjenesteplanId = parseInt(this.props.match.params.id, 10);
      const { registration } = this.state;
      if (registration.legeId) {
        this.props.registerLege(tjenesteplanId, registration);
        let legeId = -1;
        if(this.props.leger.length > 1) {
          legeId = this.props.leger.find(l => l.id != registration.legeId).id;
        }
        this.setState({
          registration: {
            legeId: legeId
          }
        });
      }
  }

  handleRemoveLege(legeId) {
    const tjenesteplanId = parseInt(this.props.match.params.id, 10);
    this.props.removeLege(tjenesteplanId, legeId);
  }

  render() {
    const id = this.props.match.params.id;

    if(this.props.isFetching || this.props.tjenesteplaner.length === 0) {
      return (
        <Loader />
      )
    } else {
      const tjenesteplanInfo = this.props.tjenesteplaner[id];
      const { registration } = this.state;

      return (
        <CenterContainerComponent>
          <div className="col-lg-6 col-md-8 col-sm-8 col-xs-8 tjenesteplanInfo">
            <div className="tjenesteplanInfo-header">
              <h1>Tjenesteplan</h1>
              <h4>{tjenesteplanInfo.name}</h4>
            </div>
            <div className="tjenesteplanInfo-addLege">
              <h3>Legg til lege</h3>

              <RegistrerLegeForm
                isFull={tjenesteplanInfo.isFull}
                legeId={registration.legeId}
                leger={this.props.leger}
                handleRegistrationChange={this.handleRegistrationChange}
                handleRegistrationSubmit={this.handleRegistrationSubmit}
              />
            </div>
            <div className="tjenesteplanInfo-leger">

              <h3>Leger i Tjenesteplan</h3>

              <LegerForm
                leger={tjenesteplanInfo.leger}
                handleRemoveLege={this.handleRemoveLege}
              />

            </div>
          </div>
        </CenterContainerComponent>
      )
    }
  }
}

function mapStateToProps(state) {
    return {
      isFetching: state.tjenesteplanInfo.isFetching,
      tjenesteplaner: state.tjenesteplanInfo.tjenesteplaner,
      leger: state.users.leger
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: (id) => {
      dispatch(tjenesteplaner.actions.getTjenesteplan(id));
      dispatch(actions.getLeger());
    },

    registerLege: (tjenesteplanId, registration) => {
      dispatch(actions.registerLege(tjenesteplanId, registration));
    },

    removeLege: (tjenesteplanId, legeId) => {
      dispatch(actions.removeLege(tjenesteplanId, legeId));
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(TjenesteplanLeger);