import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Loader } from '../_components/Loader.jsx';
import usersApi from '../Api/Users';
import { breadcrumbActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';

class Leger extends React.Component {

  constructor(props) {
    super(props);

    this.initBreadcrumbs = this.initBreadcrumbs.bind(this);
    this.removeLegeFromAvdeling = this.removeLegeFromAvdeling.bind(this);
  }

  componentDidMount() {
    this.props.initialize()
      .then(() => {
        this.initBreadcrumbs();
      });
  }

  removeLegeFromAvdeling(lege) {
    const avdelingId = parseInt(this.props.match.params.avdelingId, 10);
    this.props.removeLegeFromAvdeling(avdelingId, lege.id)
      .then(() => {
        this.initBreadcrumbs();
      });
  }

  initBreadcrumbs() {
    const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
    const avdelingId = parseInt(this.props.match.params.avdelingId, 10);

    const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
    const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
    this.props.initBreadcrumbs(sykehus, avdeling);
  }

  render() {
    if(this.props.isFetching) {
      return <Loader />;
    }

    const sykehusId = this.props.match.params.sykehusId;
    const avdelingId = this.props.match.params.avdelingId;

    return (
      <div>
        <CenteredContent>
          <div className="avdeling-leger">
            <h1>Leger</h1>
            <LinkBox
              title="+ Legg til lege"
              link={"/sykehus/" + sykehusId + "/avdelinger/" + avdelingId + "/leger/legg-til-lege"}
            />
            <div>
              {this.props.leger
                .filter(l => l.avdelinger.find(id => id == avdelingId))
                .map((lege, index) => (
                  <LegeBox lege={lege} key={index} onClick={() => this.removeLegeFromAvdeling(lege)} />
              ))}
            </div>
          </div>
        </CenteredContent>
      </div>
    );
  }
}

function CenteredContent(props) {
  return (
    <div className="absolute-position fill-height fill-width">
      <div className="container fill-height fill-width">
          <div className="align-items-center justify-content-center fill-height fill-width">
              {props.children}
          </div>
      </div>
    </div>
  )
}

function LinkBox(props) {
  const { title, link } = props;

  return (
    <Link to={link} className="btn btn-primary btn-block">
        <div>
            <span>{title}</span>
        </div>
    </Link>
)
}


function LegeBox(props) {
  const { lege, onClick } = props;

  return (
      <div className="lege-box">
          <div>
              <span>{lege.fullname}</span>
              <div className="lege-box-links">
                <a href="#" onClick={onClick}>Fjern fra avdeling</a>
              </div>
          </div>
      </div>
  )
}

function mapStateToProps(state) {
    return {
      sykehus: state.sykehus.sykehus,
      isFetching: state.users.loading,
      leger: state.users.leger
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: () => {
      dispatch(usersApi.actions.getLeger());
      return dispatch(sykehusApi.actions.getSykehus());
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

    removeLegeFromAvdeling(avdelingId, legeId) {
      return dispatch(usersApi.actions.removeFromAvdeling(avdelingId, legeId))
        .then(() => {
          return dispatch(usersApi.actions.getLeger());
        });
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Leger);