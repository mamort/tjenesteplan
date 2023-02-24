import React from 'react';
import { connect } from 'react-redux';
import { Link, NavLink } from 'react-router-dom';
import { roleConstants } from '../_constants/role.constants';
import { history } from '../_helpers';
import notificationsApi from '../Api/Notifications';

class Notifications extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isExpanded: true
        };

        history.listen((location, action) => {
            this.setState({
                isExpanded: false
            });
        });

        this.onClose = this.onClose.bind(this);
    }

    componentDidMount(){
        this.props.getNotifications();
    }

    onClose(id) {
        this.props.markNotificationAsRead(id);
    }

    render() {
      const { isExpanded, notifications } = this.props;
      if(!isExpanded || notifications.length === 0) {
          return (<div/>);
      }

      return (
            <div className="tjenesteplan-notifications">
                {notifications.map((notification) => (
                    <Notification
                        key={notification.id}
                        id={notification.id}
                        header={notification.header}
                        body={notification.body}
                        onClose={this.onClose}
                    />
                ))}
            </div>
        );
    }
  }

  function Notification(props) {
    const { date, id, header, body, onClose } = props;
    return (
        <div className="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div className="toast-header">
                <i className="dripicons-checkmark" />
                <strong className="mr-auto">{header}</strong>
                <small className="text-muted">{date}</small>
                <button
                    type="button"
                    className="ml-2 mb-1 close"
                    data-dismiss="toast"
                    aria-label="Close"
                    onClick={() => onClose(id)}
                >
                <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div className="toast-body" dangerouslySetInnerHTML={{__html: body}}>
            </div>
        </div>
    )
  }

  function createMarkup(body) {
    return {__html: body};
  }

function mapStateToProps(state) {
    return {
      isFetching: state.notifications.isFetching,
      notifications: state.notifications.notifications
    };
}

const mapDispatchToProps = (dispatch) => {
  return {
    getNotifications: () => {
      dispatch(notificationsApi.actions.getNotifications());
    },

    markNotificationAsRead(id) {
        dispatch(notificationsApi.actions.setNotificationRead(id));
    }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Notifications);