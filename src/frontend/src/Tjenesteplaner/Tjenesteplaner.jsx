import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Loader } from '../_components/Loader.jsx';
import tjenesteplaner from '../Api/Tjenesteplaner';
import { breadcrumbActions } from '../_actions';
import sykehusApi from '../Api/Sykehus';

class Tjenesteplaner extends React.Component {

  constructor(props) {
    super(props);
  }

  componentDidMount() {
    this.props.initialize()
      .then(() => {
        const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
        const avdelingId = parseInt(this.props.match.params.avdelingId, 10);

        const sykehus = this.props.sykehus.find(s => s.id == sykehusId);
        const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);
        this.props.initBreadcrumbs(sykehus, avdeling);
      });
  }

  render() {
    if(this.props.isFetching) {
      return <Loader />;
    }

    const sykehusId = this.props.match.params.sykehusId;
    const avdelingId = this.props.match.params.avdelingId;

    return (
      <div className="tjenesteplaner">
        <CenteredContent>
          <div>
            <h1>Tjenesteplaner</h1>

            <LinkButtonBox
                title="+ Opprett tjenesteplan"
                link={"/sykehus/" + sykehusId + "/avdelinger/" + avdelingId + "/opprett-tjenesteplan"}
            />

            <div>
              {this.props.tjenesteplaner
                .filter(t => t.avdelingId == avdelingId)
                .map((tjenesteplan, index) => (
                  <BoxLink tjenesteplan={tjenesteplan} key={index} />
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

function LinkButtonBox(props) {
  const { title, link } = props;

  return (
    <Link to={link} className="btn btn-primary btn-block">
        <div>
            <span>{title}</span>
        </div>
    </Link>
  )
}

function BoxLink(props) {
  const { tjenesteplan } = props;

  return (
      <div className="tjenesteplan-box">
          <div>
              <span><Link to={"tjenesteplaner/"+tjenesteplan.id}>{tjenesteplan.name}</Link></span>
              <div className="tjenesteplan-box-links">
                <Link to={"tjenesteplaner/"+tjenesteplan.id + "/ukentlig"}>Ukentlig visning</Link>
                <Link to={"tjenesteplaner/"+tjenesteplan.id + "/vakansvakter"}>Ledige vakansvakter</Link>
                <Link to={"tjenesteplaner/"+tjenesteplan.id + "/vakansvakter/aksepter"}>Godkjenn vakansvakter</Link>
                <Link to={"tjenesteplaner/"+tjenesteplan.id + "/endringer"}>Endringer</Link>
              </div>
          </div>
      </div>
  )
}

function mapStateToProps(state) {
    return {
      sykehus: state.sykehus.sykehus,
      isFetching: state.tjenesteplaner.isFetching,
      tjenesteplaner: state.tjenesteplaner.tjenesteplaner
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: () => {
      dispatch(tjenesteplaner.actions.getTjenesteplaner());
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
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Tjenesteplaner);