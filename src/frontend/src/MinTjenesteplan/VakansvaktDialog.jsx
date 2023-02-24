import React from 'react';
import { connect } from 'react-redux';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { dagsplanConstants } from '../_constants/dagsplan.constants';

class VakansvaktDialog extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
        newDagsplanId: dagsplanConstants.KursKonferanseMøte,
        reason: ''
    };

    this.handleChange = this.handleChange.bind(this);
}

  handleChange(e) {
    const { name, value } = e.target;
    this.setState({ [name]:  parseInt(value, 10) });
  }

  render() {
    let formattedDate = "";
    if(this.props.date !== null) {
      formattedDate = this.props.date.format("dddd, Do MMMM YYYY");
    }

    return (
      <div>
        <Modal isOpen={this.props.isOpen} toggle={this.props.onCancel}>
          <ModalHeader toggle={this.props.onCancel}>Vakansvakt</ModalHeader>
          <ModalBody>
                <div className="vakansvaktdialog">
                    <div className="form-group">
                      <span>Vil du sende forespørsel om vakansvakt {formattedDate}?</span>
                    </div>
                    <div className="form-group">
                      <span>Årsak</span>
                      <select
                        className="form-control"
                        name="newDagsplanId"
                        value={this.state.newDagsplanId}
                        onChange={this.handleChange}
                      >
                        <option value={dagsplanConstants.KursKonferanseMøte}>Kurs/Konferanse/Møte</option>
                        <option value={dagsplanConstants.Sykdom}>Sykdom</option>
                      </select>
                    </div>
                    <div className="form-group vakansvaktdialog-reason">
                      <span>Melding</span>
                      <textarea
                        name="reason"
                        value={this.state.reason}
                        onChange={this.handleChange}
                      />
                    </div>
                </div>
          </ModalBody>
          <ModalFooter>
            <Button onClick={() => this.props.onSubmit(this.state.newDagsplanId, this.state.reason)}>Send forespørsel</Button>
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

const connectedVakansvaktDialog = connect(mapStateToProps)(VakansvaktDialog);
export { connectedVakansvaktDialog as VakansvaktDialog };