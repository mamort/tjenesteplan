import React from 'react';
import { connect } from 'react-redux';
import { CenteredComponent } from '../_components';

class NotFoundPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const message = this.props.message;
        return (
            <div className="notfound-message">
                <CenteredComponent className="col-lg-6 col-md-8 col-sm-8 col-xs-8">
                    <div className="alert alert-secondary" role="alert">
                        <span className="notfound-404">404 - </span><span>{message}</span>
                    </div>
                </CenteredComponent>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
    };
}

const connectedNotFoundPage = connect(mapStateToProps)(NotFoundPage);
export { connectedNotFoundPage as NotFoundPage };