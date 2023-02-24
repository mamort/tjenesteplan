import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { Loader } from '../_components/Loader.jsx';
import sykehusApi from '../Api/Sykehus';
import { userActions } from '../_actions';
import { roleConstants } from '../_constants/role.constants';
import { breadcrumbActions } from '../_actions';

class Sykehus extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            selectedSykehus: null
        };
      }

    componentDidMount() {
        const self = this;
        const sykehusId = parseInt(this.props.match.params.id, 10);

        this.props.initialize()
            .then(() => {
                if(sykehusId) {
                    const sykehus = self.props.sykehus.find(s => s.id == sykehusId);
                    self.setState({
                        selectedSykehus: sykehus,
                        avdelinger: sykehus.avdelinger
                            .filter(a => a.listeforerId == this.props.user.id ||
                                this.props.user.role === roleConstants.Admin
                            )
                    });
                }
            });
    }

    render() {
        const sykehus = this.state.selectedSykehus;
        const avdelinger = this.state.avdelinger;

        if(this.props.isFetching || !this.props.user) {
            return <Loader />;
        }

        return (
            <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                <div className="sykehus">
                    <SykehusComponent user={this.props.user} sykehus={sykehus} avdelinger={avdelinger}/>
                </div>
            </CenteredComponent>
        );
    }
}

function SykehusComponent(props) {
    const { user, sykehus, avdelinger } = props;

    if(!sykehus || user.role === roleConstants.Lege) {
        return (<span>Du har ikke tilgang til informasjon om dette sykehuset.</span>);
    }


    if(sykehus.avdelinger.length === 0 && user.role !== roleConstants.Admin) {
        return (<span>Du har ikke tilgang til noen sykehusavdelinger.</span>);
    }

    return (
        <div className="center-content">
            <h1>
                {user && user.role === roleConstants.Admin
                    ? <Link to={"/sykehus/" + sykehus.id + "/rediger"}>{sykehus.name}</Link>
                    : sykehus.name
                }
            </h1>

            {user && user.role === roleConstants.Admin
                ? <BoxLinkPrimary key="1000" title="+ opprett avdeling" link={"/sykehus/" + sykehus.id + "/opprett-avdeling"} />
                : null
            }

            <div className="center-content">
                {avdelinger.map((avdeling) =>
                    <BoxLink key={avdeling.id} title={avdeling.name} link={"/sykehus/" + sykehus.id + "/avdelinger/" + avdeling.id} />
                )}
            </div>

        </div>
    );
}

function BoxLinkPrimary(props) {
    const { title, link } = props;

    return (
        <Link to={link} className="homepage-box-link btn btn-primary btn-block">
            <div>
                <span>{title}</span>
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
        isFetching: state.sykehus.isFetching || state.users.loading
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
      initialize: () => {

        dispatch(
            breadcrumbActions.set(
                [{ name: "Sykehus", link: "/sykehus" }]
            )
        );

        return Promise.all([
            dispatch(userActions.getCurrentUser()),
            dispatch(sykehusApi.actions.getSykehus())
        ]);
      },
    }
  }

const connectedSykehus = connect(mapStateToProps, mapDispatchToProps)(Sykehus);
export { connectedSykehus as Sykehus };