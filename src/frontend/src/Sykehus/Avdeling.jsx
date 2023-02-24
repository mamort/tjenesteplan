import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { Loader } from '../_components/Loader.jsx';
import sykehusApi from '../Api/Sykehus';
import usersApi from '../Api/Users';
import { userActions } from '../_actions';
import { roleConstants } from '../_constants/role.constants';
import { breadcrumbActions } from '../_actions';

class Avdeling extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            selectedSykehus: null
        };
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
                    const listeforer = self.props.leger.find(l => l.id == avdeling.listeforerId);

                    this.props.initBreadcrumbs(sykehus);

                    self.setState({
                        selectedSykehus: sykehus,
                        selectedAvdeling: avdeling,
                        listeforer: listeforer
                    });
                }
            });

        // For some reason we get an infitite loop without this check...
        if(!this.props.user){
            this.props.initializeCurrentUser();
        }
    }

    render() {
        const sykehus = this.state.selectedSykehus;
        const avdeling = this.state.selectedAvdeling;

        if(!avdeling) {
            return <Loader />;
        }

        return (
            <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                <div className="avdeling center-content">
                    <h1>
                        <Link
                            to={"/sykehus/" + sykehus.id + "/avdelinger/" + avdeling.id + "/rediger"}>
                                {avdeling.name}
                        </Link>
                    </h1>
                    <div className="center-content">
                        <Listeforer
                            user={this.props.user}
                            sykehus={sykehus}
                            avdeling={avdeling}
                            listeforer={this.state.listeforer}
                        />

                        <BoxLink
                            title="Tjenesteplaner"
                            link={"/sykehus/" + sykehus.id + "/avdelinger/" + avdeling.id + "/tjenesteplaner"}
                        />


                        <BoxLink
                            title="Leger"
                            link={"/sykehus/" + sykehus.id + "/avdelinger/" + avdeling.id + "/leger"}
                        />
                    </div>
                </div>
            </CenteredComponent>
        );
    }
}

function Listeforer(props) {
    const { user, sykehus, avdeling, listeforer } = props;

    if(!user || user.role !== roleConstants.Admin) {
        return null;
    }

    const link = "/sykehus/" + sykehus.id + "/avdelinger/" + avdeling.id + "/tildel-listeforer";

    if(!listeforer) {
        return (
            <BoxLink
                title="Tildel listefører"
                link={link}
            />
        );
    }

    return (
        <Link to={link} className="btn btn-secondary btn-block avdeling-listeforer-box">
            <div>Listefører</div>
            <div>
                <span>{listeforer.fullname}</span>
            </div>
        </Link>
    )
}

function BoxLink(props) {
    const { title, link } = props;

    return (
        <Link to={link} className="homepage-box-link btn btn-secondary btn-block">
            <div>
                <span>{title}</span>
            </div>
        </Link>
    )
}


function mapStateToProps(state) {
    return {
        user: state.users.currentUser,
        sykehus: state.sykehus.sykehus,
        leger: state.users.leger,
        isFetching: state.sykehus.isFetching ||
                    state.users.isFetching
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

      initializeCurrentUser: () => {
        return dispatch(userActions.getCurrentUser());
      },

      initBreadcrumbs: (sykehus) => {
        dispatch(
            breadcrumbActions.set(
                [
                    { name: "Sykehus", link: "/sykehus"},
                    { name: sykehus.name, link: "/sykehus/" + sykehus.id }
                ]
            )
        );
    }
    }
  }

const connectedAvdeling = connect(mapStateToProps, mapDispatchToProps)(Avdeling);
export { connectedAvdeling as Avdeling };