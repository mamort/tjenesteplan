import React from 'react';
import { connect } from 'react-redux';
import * as actions from './inviteUser.actions';
import { Loader } from '../_components/Loader.jsx';
import Select from 'react-select';
import { roleConstants } from '../_constants/role.constants';
import sykehusApi from '../Api/Sykehus';
import { alertActions } from '../_actions';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';

class InviteUser extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      invitation: {
          sykehusId: null,
          avdelingId: null,
          firstname: '',
          lastname: '',
          email: ''
      }
    };

    this.handleInvitationChange = this.handleInvitationChange.bind(this);
    this.handleInvitationEmailChange = this.handleInvitationEmailChange.bind(this);
    this.handleInvitationSubmit = this.handleInvitationSubmit.bind(this);
  }

  componentDidMount() {
    this.props.initialize()
      .then(() => {
        if(this.props.sykehus && this.props.sykehus.length > 0) {
          this.setState({
            invitation: {
                ...this.state.invitation,
                sykehusId: this.props.sykehus[0].id
            }
          });
        }
      });
  }

  handleInvitationChange(event) {
    const { name, value } = event.target;
    const { invitation } = this.state;
    this.setState({
        invitation: {
            ...invitation,
            [name]: value
        }
    });
  }

  handleInvitationEmailChange(event) {
    const { name, value } = event.target;
    const { invitation } = this.state;
    this.setState({
        invitation: {
            ...invitation,
            [name]: value.toLowerCase()
        }
    });
  }

  handleInvitationSubmit(event) {
      event.preventDefault();

      const { invitation } = this.state;
      if (invitation.email && invitation.avdelingId) {
        this.props.registerInvitation(invitation)
          .then(() => {
            this.setState({
              invitation: {
                  ...this.state.invitation,
                  fornavn: '',
                  etternavn: '',
                  email: '',
                  legeSpesialitet: null
              }
            });
          });
      }
  }

  render() {
    const invitations = this.props.invitations;

    if(this.props.isFetching) {
      return <Loader />
    }

    const { invitation } = this.state;
    const { sykehus } = this.props;

    const selectedSykehus = sykehus.find(s => s.id == invitation.sykehusId);

    return (
      <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
            <div className="inviteUser">

            <div>
              <h1>Inviter lege</h1>

              <form name="form" onSubmit={this.handleInvitationSubmit}>
                <div className="form-group">
                    <label htmlFor="sykehusId">Sykehus</label>
                    <SelectInput
                      name="sykehusId"
                      label="Velg sykehus"
                      selectedValue={invitation.sykehusId}
                      handleInvitationChange={this.handleInvitationChange}
                      options={sykehus.map(s => ({ value: s.id, label: s.name }))}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="avdelingId">Avdeling</label>
                    <SelectInput
                      name="avdelingId"
                      label="Velg avdeling"
                      selectedValue={invitation.avdelingId}
                      handleInvitationChange={this.handleInvitationChange}
                      options={
                        selectedSykehus
                          ? selectedSykehus.avdelinger.map(a => ({ value: a.id, label: a.name }))
                          : []
                        }
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="firstname">Fornavn</label>
                    <input
                      type="text"
                      className="form-control"
                      name="firstname"
                      value={invitation.firstname}
                      onChange={this.handleInvitationChange}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="lastname">Etternavn</label>
                    <input
                      type="text"
                      className="form-control"
                      name="lastname"
                      value={invitation.lastname}
                      onChange={this.handleInvitationChange}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="email">E-post</label>
                    <input
                      type="text"
                      className="form-control"
                      name="email"
                      value={invitation.email}
                      onChange={this.handleInvitationEmailChange}
                    />
                </div>

                <div className="form-group">
                    <button className="btn btn-primary">Send invitasjon</button>
                </div>
              </form>
              </div>
              <div>
                <h1>Ubesvarte invitasjoner</h1>
                <table>
                  <tbody>
                    {invitations.items.map((i) =>
                        <tr key={i.id}>
                          <td>{i.email}</td>
                        </tr>
                    )}
                  </tbody>
                </table>
              </div>

            </div>
      </CenteredComponent>
    )

  }
}

function SelectInput(props) {
  const { name, options, label, selectedValue, handleInvitationChange } = props;

  let selectedOption = { value: -1, label: label };

  if(selectedValue){
    selectedOption = options.find(o => o.value == selectedValue);
  }else{
    options.push(selectedOption);
  }

  return (<Select
    name={name}
    defaultValue={selectedValue}
    value={selectedOption}
    options={options}
    onChange={event => handleInvitationChange({target: { name, value: event.value }})}
    hideSelectedOptions={true}
  />);
}

function mapStateToProps(state) {
    return {
      invitations: state.invitations,
      loggedInUser: state.authentication.user,
      sykehus: state.sykehus.sykehus,
      isFetching: state.sykehus.isFetching ||
                  state.invitations.isFetching
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: () => {
      return Promise.all([
        dispatch(sykehusApi.actions.getSykehus()),
        dispatch(actions.getInvitations())
      ]);
    },

    registerInvitation: (invitation) => {
      return dispatch(actions.registerInvitation(invitation))
        .then(() => {
          dispatch(alertActions.success("Ny lege er invitert."));
          dispatch(actions.getInvitations());
        })
        .catch(error => {
          if(error.httpStatusCode == 409){
            dispatch(alertActions.error("Lege er allerede registrert."));
          } else {
            dispatch(alertActions.error("Kunne ikke registere lege pga en feil."));
          }
          throw error;
        });
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(InviteUser);