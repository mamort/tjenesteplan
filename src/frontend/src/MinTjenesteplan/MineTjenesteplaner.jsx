import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Loader } from '../_components/Loader.jsx';
import { history } from '../_helpers';
import minTjenesteplan from '../Api/MinTjenesteplan';
import sykehusApi from '../Api/Sykehus';

class MineTjenesteplaner extends React.Component {

  constructor(props) {
    super(props);
  }

  componentDidMount() {
      this.props.initialize();
  }

  render() {
    if(this.props.isFetching) {
      return <Loader />;
    }

    const sykehus = this.props.sykehus;
    const tjenesteplaner = this.props.tjenesteplaner;

    return (
      <div className="minetjenesteplaner">
        <CenteredContent>
          <div>
            <h1>Tjenesteplaner</h1>

            <div className="tjenesteplaner minetjenesteplaner-list">
              {sykehus.map((s, index) => (
                <SykehusTjenesteplaner
                  key={index}
                  sykehus={s}
                  tjenesteplaner={tjenesteplaner.filter(t => s.avdelinger.filter(a => a.id == t.avdelingId).length > 0)}
                />
              ))}
            </div>

            {tjenesteplaner.length === 0
              ? <span>Du har ingen tjenesteplaner</span>
              : null
            }
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

function SykehusTjenesteplaner(props) {
  const { sykehus, tjenesteplaner } = props;

  return (
    <div className="minetjenesteplaner-sykehus">
      <h3>{sykehus.name}</h3>
      {tjenesteplaner.map((tjenesteplan, index) => (
        <TjenesteplanBox sykehus={sykehus} tjenesteplan={tjenesteplan} key={index} />
      ))}
    </div>
  )
}

function TjenesteplanBox(props) {
  const { sykehus, tjenesteplan } = props;

  const avdeling = sykehus.avdelinger.find(a => a.id == tjenesteplan.avdelingId);

  return (
      <div className="tjenesteplan-box">
          <div>
              <Link to={"minetjenesteplaner/"+tjenesteplan.id}>Tjenesteplan {tjenesteplan.name}</Link>
              <span className="minetjenesteplaner-sykehus-avdeling">{avdeling.name}</span>
              <div className="tjenesteplan-box-links">
                <Link to={"minetjenesteplaner/"+tjenesteplan.id + "/vakansvakter"}>Ledige vakansvakter</Link>
                <Link to={"minetjenesteplaner/"+tjenesteplan.id + "/mine-vakansvaktforespørsler"}>Mine vakansvaktforespørsler</Link>
                <Link to={"minetjenesteplaner/"+tjenesteplan.id + "/vaktbytter/aksepter"}>Mine vaktbytter</Link>
                <Link to={"minetjenesteplaner/"+tjenesteplan.id + "/vaktbytter"}>Svar på forespørsler om vaktbytte</Link>
              </div>
          </div>
      </div>
  )
}

function mapStateToProps(state) {
    return {
        isFetching: state.mintjenesteplan.minetjenesteplaner.isFetching || state.sykehus.isFetching,
        tjenesteplaner: state.mintjenesteplan.minetjenesteplaner.tjenesteplaner,
        sykehus: state.sykehus.sykehus
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: () => {
        dispatch(minTjenesteplan.actions.getMineTjenesteplaner());
        dispatch(sykehusApi.actions.getSykehus());
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(MineTjenesteplaner);