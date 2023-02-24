import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { CenteredComponent } from '../_components';

class Overlege extends React.Component {
    render() {
        const { user } = this.props;

        return (
            <CenteredComponent className="col-lg-8 col-md-8 col-sm-8 col-xs-8">
                <Content user={user} />
            </CenteredComponent>
        );
    }
}

function Content(props) {
    const { user } = props;

    return (
        <div>
            <BoxLink title="Inviter lege" link="/Inviter" />
            <BoxLink title="Sykehus" link="/Sykehus" />
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

const connectedOverlege = connect(mapStateToProps)(Overlege);
export { connectedOverlege as Overlege };