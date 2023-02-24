import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Loader } from '../_components/Loader.jsx';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import usersApi from '../Api/Users';
import { breadcrumbActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';
import legeSpesialiteterApi from '../Api/LegeSpesialiteter';

class LegeProfile extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      legeSpesialitet: null,
      sykehus: []
    }
  }

  componentDidMount() {
    const userId = parseInt(this.props.match.params.userId, 10);
    this.props.initialize(userId)
      .then(() => {
        const lege = this.props.lege;
        const legeSpesialitet = this.props.legeSpesialiteter.find(s => s.id == lege.spesialitetId);
        const sykehus = this.props.sykehus.map(s => {
            const avdelinger = s.avdelinger.filter(a => lege.avdelinger.includes(a.id));

            return {
              id: s.id,
              name: s.name,
              avdelinger: avdelinger
            }
          })
          .filter(s => s.avdelinger.length > 0);

          this.setState({
            sykehus,
            legeSpesialitet
          });
      });
  }

  render() {
    if(this.props.error && this.props.error.httpStatusCode === 403) {
      return (
        <CenteredComponent>
          <h1>Ingen tilgang</h1>
        </CenteredComponent>
      );
    }

    if(this.props.isFetching || !this.props.lege) {
      return <Loader />;
    }

    const lege = this.props.lege;
    const legeSpesialitet = this.state.legeSpesialitet;
    const sykehus = this.state.sykehus;

    return (
      <CenteredComponent>
        <div className="legeProfile">
          <h2>Profil</h2>
          <table className="table table-striped">
            <tbody>
              <tr>
                <td>Navn</td>
                <td className="legeProfile-value">{lege.fullname}</td>
              </tr>
              <tr>
                <td>E-post</td>
                <td className="legeProfile-value"><a href={"mailto:" + lege.email}>{lege.email}</a></td>
              </tr>
              <tr>
                <td>Spesialitet</td>
                <td className="legeProfile-value">{legeSpesialitet ? legeSpesialitet.name : ""}</td>
              </tr>
            </tbody>
          </table>

          <h2>Sykehus</h2>
          {sykehus.map(s => (
            <table className="table table-striped">
              <tbody>
              <tr>
                <td>{s.name}</td>
                <td></td>
              </tr>
              <tr>
                <td>Avdelinger</td>
                <td className="legeProfile-avdeling">
                  <ul>
                    {s.avdelinger.map(a =>
                      (<li key={a.id}>{a.name}</li>)
                    )}
                  </ul>
                </td>
              </tr>
              </tbody>
            </table>
            ))
          }
        </div>
      </CenteredComponent>
    );
  }
}

function mapStateToProps(state) {
    return {
      sykehus: state.sykehus.sykehus,
      isFetching: state.users.loading || state.legeSpesialiteter.isFetching,
      lege: state.users.lege,
      legeSpesialiteter: state.legeSpesialiteter.spesialiteter,
      error: state.users.error
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: (userId) => {
      return Promise.all([
          dispatch(usersApi.actions.getLege(userId)),
          dispatch(legeSpesialiteterApi.actions.getLegeSpesialiteter()),
          dispatch(sykehusApi.actions.getSykehus())
        ]);
    },

    initBreadcrumbs: (sykehus, avdeling) => {
      dispatch(
          breadcrumbActions.set(
            [
              { name: "Sykehus", link: "/sykehus"},
              { name: sykehus.name, link: "/sykehus/" + sykehus.id },
              { name: avdeling.name, link: "/sykehus/" + sykehus.id + '/avdelinger/' + avdeling.id + '/leger' }
            ]
          )
      );
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(LegeProfile);