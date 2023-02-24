import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { CenteredComponent } from '../_components/CenteredComponent.jsx';
import { Loader } from '../_components/Loader.jsx';
import sykehusApi from '../Api/Sykehus';
import { userActions } from '../_actions';
import { roleConstants } from '../_constants/role.constants';

class SykehusList extends React.Component {

    constructor(props) {
        super(props);
      }

    componentDidMount() {
        this.props.initialize();

        // For some reason we get an infitite loop without this check...
        if(!this.props.user){
            this.props.initializeCurrentUser();
        }
    }

    render() {
        if(this.props.isFetching || !this.props.user) {
            return <Loader />;
        }

        const sykehus = this.props.sykehus;

        return (
            <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                <div className="sykehus">
                    <h1>Sykehus</h1>
                    <Sykehus user={this.props.user} sykehus={sykehus} />
                </div>
            </CenteredComponent>
        );
    }
}

function Sykehus(props) {
    const { user, sykehus } = props;

    if((!sykehus || sykehus.length === 0) && user.role !== roleConstants.Admin) {
        return (<span>Du har ikke tilgang til noen sykehus.</span>);
    }

    return (
        <div className="center-content">
            {user && user.role === roleConstants.Admin
                ? <BoxLinkPrimary key="1000" title="+ opprett sykehus" link={"opprett-sykehus"} />
                : null
            }

            <div className="center-content">
                {sykehus.map((sykehus) =>
                    <BoxLink key={sykehus.id} title={sykehus.name} link={"/sykehus/" + sykehus.id} />
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
          return dispatch(sykehusApi.actions.getSykehus());
      },

      initializeCurrentUser: () => {
        return dispatch(userActions.getCurrentUser());
      }
    }
  }

const connectedSykehusList = connect(mapStateToProps, mapDispatchToProps)(SykehusList);
export { connectedSykehusList as SykehusList };