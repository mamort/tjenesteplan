import React from 'react';
import { CenteredComponent } from '../_components';

class Loader extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <CenteredComponent>
                <div>
                    <div className="loader">
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                        <div></div>
                    </div>
                    <div className="loader-text">Et lite øyeblikk...</div>
                </div>
            </CenteredComponent>
        );
    }
}

export { Loader };