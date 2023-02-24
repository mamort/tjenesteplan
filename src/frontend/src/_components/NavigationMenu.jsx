import React from 'react';
import { connect } from 'react-redux';
import { Link, NavLink } from 'react-router-dom';
import { roleConstants } from '../_constants/role.constants';
import { history } from '../_helpers';
import Notifications from './Notifications';
import { breadcrumbActions } from '../_actions';

class NavigationMenu extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isExpanded: false,
            isNotificationsExpanded: false
        };

        this.onDocumentClick = this.onDocumentClick.bind(this);
        this.notificationsRef = React.createRef();
        this.onNavigationChange = this.onNavigationChange.bind(this);
    }

    componentDidMount() {
        this.unsubscribeFromHistory = history.listen(this.onNavigationChange);
        document.getElementsByTagName('body')[0].addEventListener('mousedown', this.onDocumentClick, false);
    }

    componentWillUnmount() {
        this.unsubscribeFromHistory();
        document.getElementsByTagName('body')[0].removeEventListener('mousedown', this.onDocumentClick);
    }

    onNavigationChange(){
        this.props.clearBreadcrumbs();

        this.setState({
            isExpanded: false
        });
    }

    handleToggle(e) {
      this.setState({
        isExpanded: !this.state.isExpanded
      });
    }

    onDocumentClick(e) {
        if(!this.notificationsRef.current.contains(e.target)) {
            this.setState({
                isNotificationsExpanded: false
              });
        }
    }

    onNotificationBellClick(){
        if(this.props.notifications.length > 0){
            this.setState({
                isNotificationsExpanded: !this.state.isNotificationsExpanded
            });
        }
    }

    render() {
        const { notifications } = this.props;
      const { isExpanded, isNotificationsExpanded } = this.state;
      const notificationsAny = notifications.length > 0;

      const breadcrumbs = this.props.breadcrumbs;

      return (
        <div className={`tjenesteplan-header ${isExpanded ? "tjenesteplan-header-menu-expanded" : ""}`}>

            <div className="tjenesteplan-hamburger-menu row justify-content-start no-gutters">
                <nav className="nav col-md-10 col-lg-10 col-sm-1 col-xs-1">
                    <ul className="tjenesteplan-header-menu">
                        <li className="tjenesteplan-header-menu-section">
                            <span>Mine sider</span>
                            <ul>
                                <NavLink activeClassName="active" to="/MineTjenesteplaner">
                                    <li>Mine tjenesteplaner</li>
                                </NavLink>
                            </ul>

                        </li>

                        {this.props.user.role === roleConstants.Admin ||
                         this.props.user.role === roleConstants.Overlege ?
                            <li className="tjenesteplan-header-menu-section">
                                <span>Administrativt</span>
                                <ul>
                                    <NavLink activeClassName="active" to="/sykehus">
                                        <li>Sykehus</li>
                                    </NavLink>
                                    <NavLink activeClassName="active" to="/Inviter">
                                        <li>Inviter lege</li>
                                    </NavLink>
                                </ul>
                            </li>
                        : null }

                        <li className="tjenesteplan-header-menu-section">
                            <span>Annet</span>
                            <ul>
                                <NavLink activeClassName="active" to="/Tjenesteplaner">
                                    <li>Kontakt</li>
                                </NavLink>
                            </ul>
                        </li>
                    </ul>
                </nav>

                <div className="tjenesteplan-hamburger-menu-logout">
                    <a className="btn btn-secondary" onClick={this.props.handleLogout}>
                        Logg ut
                    </a>
                </div>
            </div>

            <div className="tjenesteplan-header-top">
                <div className="tjenesteplan-hamburger">
                    <input type="checkbox" onChange={e => this.handleToggle(e)} checked={isExpanded}/>
                    <span />
                    <span />
                    <span />
                    <div />
                </div>
                <div className="tjenesteplan-header-top-wrapper">
                    <div className="tjenesteplan-header-logo">
                        <Link to="/">Tjenesteplan</Link>
                    </div>
                    <div ref={this.notificationsRef} className={`tjenesteplan-header-notifications ${notificationsAny ? "tjenesteplan-header-notifications--any" : ""}`}>
                        {notificationsAny
                            ? <span className="badge badge-light"  onClick={() => this.onNotificationBellClick()}>{notifications.length}</span>
                            : <span />
                        }
                        <i className={`dripicons-bell ${notificationsAny ? "" : ""}`} onClick={() => this.onNotificationBellClick()} />
                        <Notifications isExpanded={isNotificationsExpanded} />
                    </div>
                    <div className="tjenesteplan-header-username">
                        <i className="dripicons-user" />
                        <span>{this.props.user.firstName}</span>
                    </div>
                </div>

          </div>

          {breadcrumbs && breadcrumbs.length >  0
            ?
                <div className="tjenesteplan-breadcrumb">
                    <i className="dripicons-chevron-right" />
                    <Breadcrumbs breadcrumbs={this.props.breadcrumbs} />
                </div>
            : null
          }
        </div>
      );
    }
  }

  function Breadcrumbs(props) {
    const { breadcrumbs } = props;

    return (
        <div>
            {breadcrumbs.map((crumb, index) => (
                <span key={index}>
                    { crumb.link
                        ? <Link to={crumb.link} className="tjenesteplan-breadcrumb-item">{crumb.name}</Link>
                        : <span className="tjenesteplan-breadcrumb-item">{crumb.name}</span>
                    }
                    {index != breadcrumbs.length-1 ? " / " : ""}
                </span>
            ))}
        </div>
    )
  }

function mapStateToProps(state) {
    return {
      isFetchingNotifications: state.notifications.isFetching,
      notifications: state.notifications.notifications,
      breadcrumbs: state.breadcrumbs.crumbs
    };
}

const mapDispatchToProps = (dispatch) => {
  return {
      clearBreadcrumbs: () => {
        dispatch(breadcrumbActions.clear());
      }
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(NavigationMenu);