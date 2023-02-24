import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { CenteredComponent } from '../_components';

class Lege extends React.Component {
    render() {
        const { user } = this.props;

        return (
            <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                <Content user={user} />
            </CenteredComponent>
        );
    }
}

function Content(props) {
    const { user } = props;

    if(user.tjenesteplaner.length === 0) {
        return (
            <div className="homepage-text">
                <h2>Velkommen til Tjenesteplan</h2>
                <p>
                    Du har ikke fått tildelt en tjenesteplan ennå.
                </p>
            </div>
        );
    }

    return (
        <div>
            <BoxLink title="Mine tjenesteplaner" link="/MineTjenesteplaner" />
        </div>
    );
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
    };
}

const connectedLege = connect(mapStateToProps)(Lege);
export { connectedLege as Lege };