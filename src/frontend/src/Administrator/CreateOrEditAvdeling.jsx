import React from 'react';
import { connect } from 'react-redux';
import { history } from '../_helpers';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import avdelingerApi from '../Api/Avdelinger';
import { Loader } from '../_components/Loader.jsx';
import sykehusApi from '../Api/Sykehus';

class CreateOrEditAvdeling extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      avdeling: {
          title: ''
      }
    };

    this.handleAvdelingChange = this.handleAvdelingChange.bind(this);
    this.handleAvdelingSubmit = this.handleAvdelingSubmit.bind(this);
    this.handleDeleteAvdelingSubmit = this.handleDeleteAvdelingSubmit.bind(this);
  }
  componentDidMount() {
    this._ismounted = true;

    const self = this;
    const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
    const avdelingId = parseInt(this.props.match.params.id, 10);

    this.props.initialize()
        .then(() => {
            if(avdelingId) {
                const sykehus  = self.props.sykehus.find(s => s.id == sykehusId);
                const avdeling = sykehus.avdelinger.find(a => a.id == avdelingId);

                self.setState({
                    avdeling: {
                        id: avdeling.id,
                        title: avdeling.name
                    }
                });
            }
        });
  }

  componentWillUnmount() {
     this._ismounted = false;
  }

  handleAvdelingChange(event) {
    const { name, value } = event.target;
    const { avdeling } = this.state;
    this.setState({
        avdeling: {
            ...avdeling,
            [name]: value
        }
    });
  }

  handleAvdelingSubmit(event) {
      event.preventDefault();

      const self = this;
      const sykehusId = parseInt(this.props.match.params.sykehusId, 10);
      const avdelingId = parseInt(this.props.match.params.id, 10);

      const { avdeling } = this.state;

      if (sykehusId && avdeling.title) {
        this.setState({
          avdeling: {
              title: ''
          },
          isSaving:true
        });

        if(avdelingId) {
          this.props.editAvdeling(sykehusId, avdeling)
            .then(() => {
              if(self._ismounted) {
                self.setState({
                  isSaving: false
                });
              }
            });
        } else {
          this.props.createAvdeling(sykehusId, avdeling)
            .then(() => {
              if(self._ismounted) {
                self.setState({
                  isSaving: false
                });
              }
            });
        }
      }
  }

  handleDeleteAvdelingSubmit(event) {
    event.preventDefault();

    const avdelingId = parseInt(this.props.match.params.id, 10);
    if(avdelingId) {
      this.props.deleteAvdeling(avdelingId);
    }
  }

  render() {

    const avdelingId = this.props.match.params.id;
    const { avdeling } = this.state;

    if(this.state.isSaving) {
      return <Loader />;
    }

    return (
      <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
            <div>
              <h1>{ avdelingId ? 'Endre avdelingsnavn' : 'Opprett ny avdeling' }</h1>

              <form name="form" onSubmit={this.handleAvdelingSubmit}>
                <div className="form-group">
                    <label htmlFor="title">Navn</label>
                    <input
                      type="text"
                      className="form-control"
                      name="title"
                      value={avdeling.title}
                      onChange={this.handleAvdelingChange}
                    />
                </div>

                <div className="form-group">
                    <button className="btn btn-primary">
                      { avdelingId ? 'Lagre' : 'Opprett avdeling' }
                    </button>
                </div>
              </form>

              { avdelingId ?
                <div>
                  <br/><br/>
                  <h2>Slett avdeling</h2>
                  <form name="form" onSubmit={this.handleDeleteAvdelingSubmit}>
                    <button className="btn btn-danger">
                        Slett avdeling
                    </button>
                  </form>
                </div>
              : null }
            </div>
          </CenteredComponent>
    )
  }
}

function mapStateToProps(state) {
    return {
      sykehus: state.sykehus.sykehus
    };
}
const mapDispatchToProps = (dispatch) => {
  return {
    initialize: () => {
      return dispatch(sykehusApi.actions.getSykehus())
    },

    createAvdeling: (sykehusId, avdeling) => {
      return dispatch(avdelingerApi.actions.createAvdeling(sykehusId, avdeling.title))
        .then(() => {
            history.push('/sykehus/' + sykehusId);
        });
    },

    editAvdeling: (sykehusId, avdeling) => {
      return dispatch(avdelingerApi.actions.updateAvdeling(avdeling.id, avdeling.title))
          .then(() => {
              history.push('/sykehus/' + sykehusId + '/avdelinger/' + avdeling.id);
          });
    },

    deleteAvdeling: (avdelingId) => {
      return dispatch(avdelingerApi.actions.deleteAvdeling(avdelingId))
      .then(() => {
          history.push('/sykehus/' + sykehusId);
      });
    }
  }
}

const connectedCreateOrEditAvdeling = connect(mapStateToProps, mapDispatchToProps)(CreateOrEditAvdeling);
export { connectedCreateOrEditAvdeling as CreateOrEditAvdeling };