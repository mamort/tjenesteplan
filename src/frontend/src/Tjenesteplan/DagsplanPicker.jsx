import React from 'react';
import { connect } from 'react-redux';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { dagsplanConstants } from '../_constants/dagsplan.constants';
import { dagsplanerActions } from '../Api/Dagsplaner';

class DagsplanPicker extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      dagsplaner: [],

      validDagsplaner : [
        dagsplanConstants.None,
        dagsplanConstants.Døgnvakt,
        dagsplanConstants.FriEtterDøgnVakt,
        dagsplanConstants.Dagvakt,
        dagsplanConstants.Avspasering,
        dagsplanConstants.FordypningForskning
      ]
    };
  }

  componentDidMount() {
    if(!this.props.dagsplaner || this.props.dagsplaner.length === 0) {
      this.props.dispatch(dagsplanerActions.getDagsplaner());
    }
  }

  componentWillReceiveProps(nextProps) {
    if (this.state.dagsplaner.length === 0 &&
        nextProps.dagsplaner && nextProps.dagsplaner.length > 0) {
      const dagsplaner = nextProps.dagsplaner
        .filter(d => d.isRolling)
        .map(d => {
          return {
            id: d.id,
            name: d.name
          }
      });

      dagsplaner.push({
        id: dagsplanConstants.None,
        name: "Vanlig arbeidsdag"
      });

      this.setState({
        dagsplaner
      });
    }
  }

  render() {
    let day = "";
    if(this.props.date) {
      day = this.props.date.format("dddd D. MMMM");
    }
    return (
      <div>
        <Modal isOpen={this.props.isOpen} toggle={this.props.onCancel} className="dagsplanPicker">
          <ModalHeader toggle={this.props.onCancel}>Velg dagsplan | {day}</ModalHeader>
          <ModalBody>
                <div>
                    <ul className="dagsplanPicker-list">
                      {this.state.dagsplaner.map((dagsplan) => Dagsplan(dagsplan, this.props.onDagsplanSelected))}
                    </ul>
                </div>
          </ModalBody>
          <ModalFooter>
            <Button onClick={this.props.onByttVakt}>Velg</Button>
            <Button onClick={this.props.onCancel}>Avbryt</Button>
          </ModalFooter>
        </Modal>
      </div>
    )
  }
}

function Dagsplan(dagsplan, onDagsplanSelected) {
  const attr = {};
  attr.key = dagsplan.id;
  attr.onClick = () => onDagsplanSelected(dagsplan.id);
  attr.className = "datepickerDay--dagsplan-" + dagsplan.id;

  return (
    <li {...attr}>{dagsplan.name}</li>
  );
}

function mapStateToProps(state) {
    return {
      dagsplaner: state.dagsplaner.alleDagsplaner
    };
}

export default connect(mapStateToProps)(DagsplanPicker);