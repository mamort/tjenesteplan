import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { userActions } from '../_actions';

class Users extends React.Component {

    constructor(props) {
        super(props);

        this.handleDeleteUser = this.handleDeleteUser.bind(this);
      }

    componentDidMount() {
        if(!this.props.users.items){
            this.props.dispatch(userActions.getAll());
        }
    }

    handleDeleteUser(id) {
        return (e) => this.props.dispatch(userActions.delete(id));
    }

    render() {
        const { users } = this.props;

        return (
            <div className="col-md-6 col-md-offset-3">
                <UserList users={users} handleDeleteUser={this.handleDeleteUser} />
            </div>
        );
    }
}

function UserList(props) {
    const { users, handleDeleteUser } = props;

    return (
        <div className="admin-landing-page">
            <br />
            <br />
            <h3>Alle registrerte brukere:</h3>
            <br />
            {users.error && <span className="text-danger">ERROR: {users.error}</span>}
            {users.items &&
            <table className="table table-borderless">
                <thead>
                    <tr>
                        <th>Brukernavn</th>
                        <th></th>
                    </tr>
                </thead>

                <tbody>
                {users.items.map((user, index) =>
                    <tr key={user.id}>
                        <td>{user.username}</td>
                        <td>
                        {
                            user.deleting ? <em> - Sletter...</em>
                            : user.deleteError ? <span className="text-danger"> - ERROR: {user.deleteError}</span>
                            : <a href="#" className="btn btn-danger" onClick={handleDeleteUser(user.id)}>Slett</a>
                        }
                        </td>
                    </tr>
                )}
                </tbody>
            </table>
            }
        </div>
    );
}

function mapStateToProps(state) {
    return {
        users: state.users
    };
}

const connectedAdmin = connect(mapStateToProps)(Admin);
export { connectedAdmin as Admin };