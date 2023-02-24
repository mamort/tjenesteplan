import React from 'react';
import { connect } from 'react-redux';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class ByttVakt extends React.Component{
  render() {
    let formattedDate = "";
    if(this.props.date !== null) {
      formattedDate = this.props.date.format("dddd, Do MMMM YYYY");
    }

    return (
      <div>
        <Modal isOpen={this.props.isOpen} toggle={this.props.onCancel}>
          <ModalHeader toggle={this.props.onCancel}>Bytte vakt</ModalHeader>
          <ModalBody>
                <div>
                  Vil du sende forespørsel om å bytte vakten {formattedDate}?
                </div>
          </ModalBody>
          <ModalFooter>
            <Button onClick={this.props.onByttVakt}>Send forespørsel</Button>
            <Button onClick={this.props.onCancel}>Avbryt</Button>
          </ModalFooter>
        </Modal>
      </div>
    )
  }
}

function mapStateToProps(state) {
    return {
    };
}

const connectedByttVakt = connect(mapStateToProps)(ByttVakt);
export { connectedByttVakt as ByttVakt };