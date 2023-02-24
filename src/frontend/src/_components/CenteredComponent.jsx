import React from 'react';

class CenteredComponent extends React.Component {
    constructor(props) {
        super(props);
    }

    // https://stackoverflow.com/questions/396145/how-to-vertically-center-a-div-for-all-browsers
    render() {
        return (
            <div className="absolute-position fill-height fill-width">
                <div className="container fill-height fill-width">
                    <div className="row justify-content-center fill-height fill-width">
                        <div className={this.props.className + " centered-content-wrapper"}>
                            <div>
                                {this.props.children}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export { CenteredComponent };