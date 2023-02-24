import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { history } from '../_helpers';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import sykehusApi from '../Api/Sykehus';

class CreateOrEditSykehus extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      sykehus: {
          title: ''
      }
    };

    this.handleSykehusChange = this.handleSykehusChange.bind(this);
    this.handleSykehusSubmit = this.handleSykehusSubmit.bind(this);
    this.handleDeleteSykehusSubmit = this.handleDeleteSykehusSubmit.bind(this);
  }

  componentDidMount() {
    const self = this;

    const sykehusId = parseInt(this.props.match.params.id, 10);

    this.props.initialize()
        .then(() => {
            if(sykehusId) {
                const sykehus  = self.props.sykehus.find(s => s.id == sykehusId);
                self.setState({
                    sykehus: {
                        id: sykehus.id,
                        title: sykehus.name
                    }
                });
            }
        });
  }

  handleSykehusChange(event) {
    const { name, value } = event.target;
    const { sykehus } = this.state;
    this.setState({
        sykehus: {
            ...sykehus,
            [name]: value
        }
    });
  }

  handleSykehusSubmit(event) {
    const self = this;

    event.preventDefault();

    const { sykehus } = this.state;

    if(sykehus.id && sykehus.title) {
        this.props.editSykehus(sykehus);
    } else if (sykehus.title) {
        this.props.createSykehus(sykehus);
    }
  }

  handleDeleteSykehusSubmit(event) {
    event.preventDefault();

    const sykehusId = parseInt(this.props.match.params.id, 10);
    if(sykehusId) {
      this.props.deleteSykehus(sykehusId);
    }
  }

  render() {
    const { sykehus } = this.state;
    const sykehusId = this.props.match.params.id;

    return (
        <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
            <div className="createEditSykehus">
                <div>
                    <h1>{ sykehusId ? "Endre sykehus navn" : "Opprett nytt sykehus" }</h1>

                    <form name="form" onSubmit={this.handleSykehusSubmit}>
                        <div className="form-group">
                            <label htmlFor="title">Navn</label>
                            <input
                            type="text"
                            className="form-control"
                            name="title"
                            value={sykehus.title}
                            onChange={this.handleSykehusChange}
                            />
                        </div>

                        <div className="form-group">
                            <button className="btn btn-primary">
                                { sykehusId ? "Lagre" : "Opprett sykehus" }
                            </button>

                            { sykehusId
                                ? <Link to={"/sykehus/" + sykehusId}>Tilbake</Link>
                                : null
                            }
                        </div>
                    </form>


                    { sykehusId ?
                        <div>
                        <br/><br/>
                        <h2>Slett sykehus</h2>
                        <form name="form" onSubmit={this.handleDeleteSykehusSubmit}>
                            <button className="btn btn-danger">
                                Slett sykehus
                            </button>
                        </form>
                        </div>
                    : null }
                </div>
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

    createSykehus: (sykehus) => {
        return dispatch(sykehusApi.actions.createSykehus(sykehus.title))
            .then(() => {
                history.push('/');
            });
    },

    editSykehus: (sykehus) => {
        return dispatch(sykehusApi.actions.editSykehus(sykehus.id, sykehus.title))
            .then(() => {
                history.push('/sykehus/' + sykehus.id);
            })
    },

    deleteSykehus: (sykehusId) => {
        return dispatch(sykehusApi.actions.deleteSykehus(sykehusId))
            .then(() => {
                history.push('/');
            });
    }
  }
}

const connectedCreateOrEditSykehus = connect(mapStateToProps, mapDispatchToProps)(CreateOrEditSykehus);
export { connectedCreateOrEditSykehus as CreateOrEditSykehus };