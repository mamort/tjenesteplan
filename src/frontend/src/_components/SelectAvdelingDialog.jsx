import React from 'react';
import { connect } from 'react-redux';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import Select from 'react-select';

class SelectAvdelingDialog extends React.Component {

  constructor(props) {
    super(props);

    this.handleAvdelingChange = this.handleAvdelingChange.bind(this);
  }

  handleAvdelingChange(avdelingId) {
    let selectedSykehus = null;
    let selectedAvdeling = null;
    for(let i = 0; i < this.props.sykehus.length; i++) {
        const shus = this.props.sykehus[i];
        selectedAvdeling = shus.avdelinger.find(a => a.id == avdelingId);
        if(selectedAvdeling) {
            selectedSykehus = shus;
            break;
        }
    }

    this.setState({
        selectedSykehus: selectedSykehus,
        selectedAvdeling: selectedAvdeling
    });
}

  render() {
    const sykehusOptions = this.props.sykehus.map(s => {
      return {
          label: s.name,
          options: s.avdelinger.map(a => {
              return {
                  label: a.name,
                  value: a.id
              }
          })
      }
    });

    return (
      <div>
        <Modal isOpen={this.props.isOpen} toggle={this.props.onCancel}>
          <ModalHeader toggle={this.props.onCancel}>Velg sykehusavdeling</ModalHeader>
          <ModalBody>
                <div className="selectAvdeling-dialog">
                  <SelectAvdeling
                      name="avdelingId"
                      label="Velg avdeling"
                      style={{width: `100%`}}
                      selectedValue={this.props.selectedAvdelingId}
                      handleAvdelingChange={this.handleAvdelingChange}
                      options={sykehusOptions}
                  />
                </div>
          </ModalBody>
          <ModalFooter>
            <Button
              onClick={() => this.props.onSubmit(
                  this.state.selectedSykehus,
                  this.state.selectedAvdeling)}
            >
                    Velg avdeling
            </Button>
            <Button onClick={this.props.onCancel}>Avbryt</Button>
          </ModalFooter>
        </Modal>
      </div>
    )
  }
}


function SelectAvdeling(props) {
  const { name, options, label, selectedValue, handleAvdelingChange } = props;

  let selectedOption = { value: -1, label: label };

  if(selectedValue){
    selectedOption = options.find(o => o.value == selectedValue);
  }else{
    options.push(selectedOption);
  }

  return (<Select
    name={name}
    placeholder="Velg avdeling"
    defaultValue={selectedValue}
    value={selectedOption}
    options={options}
    onChange={event => handleAvdelingChange(event.value)}
    hideSelectedOptions={true}
  />);
}



function mapStateToProps(state) {
    return {
    };
}

const connectedSelectAvdelingDialog= connect(mapStateToProps)(SelectAvdelingDialog);
export { connectedSelectAvdelingDialog as SelectAvdelingDialog };