import React from 'react';
import { connect } from 'react-redux';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

class CategoryPicker extends React.Component{
  render() {
    return (
      <div>
        <Modal isOpen={this.props.isOpen} toggle={this.props.onCancel}>
          <ModalHeader toggle={this.props.onCancel}>Velg kategori</ModalHeader>
          <ModalBody>
                <ul>
                    {this.props.categories.map((category, index) => (
                        <li onClick={() => this.props.onSelectCategory(category.id)} key={index}>{category.name}</li>
                    ))}
                </ul>
          </ModalBody>
          <ModalFooter>
            <Button onClick={this.props.onCancel}>Cancel</Button>
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

const connectedCategoryPicker = connect(mapStateToProps)(CategoryPicker);
export { connectedCategoryPicker as CategoryPicker };