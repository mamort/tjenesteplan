import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { roleConstants } from '../_constants/role.constants';
import { SykehusList } from '../Sykehus';
import { Lege } from './Lege';
import { Overlege } from './Overlege';
import { userActions } from '../_actions';
import { Loader } from '../_components/Loader.jsx';

class HomePage extends React.Component {
    componentDidMount() {
        if(!this.props.user){
            this.props.dispatch(userActions.getCurrentUser());
        }
    }

    render() {
        const { isLoading, user } = this.props;

        if(isLoading || !user) {
            return <Loader />;
        }

        if(user.role === roleConstants.Lege) {
            return <Lege user={user} />;
        }

        if(user.role === roleConstants.Overlege) {
            return <Overlege user={user} />;
        }

        return <SykehusList user={user} />;
    }
}

function mapStateToProps(state) {
    return {
        isLoading: state.users.loading,
        user: state.users.currentUser
    };
}

const connectedHomePage = connect(mapStateToProps)(HomePage);
export { connectedHomePage as HomePage };