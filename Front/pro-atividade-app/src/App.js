import './App.css';
import  { useState, useEffect }  from 'react';
import {Button,Modal}from 'react-bootstrap'
import AtividadeForm from './components/AtividadeForm';
import AtividadeLista from './components/AtividadeLista';
import api from './api/atividade'



function App() {
    const [showAtividadeModal, setStateModal] = useState(false);
    const [smShowConfirmModal, setConfirmModal] = useState(false);
    const [atividades, setAtividades] = useState([]);
    const [atividade, setAtividade] = useState({ id: 0 });

    const handleStateModal = () => setStateModal(!showAtividadeModal);
    const handleConfirmModal = (id) => {

         if(id !== 0 && id !== undefined){
          const atividade = atividades.filter((atividade) => atividade.id === id);
          setAtividade(atividade[0]);
     }
     else{
      setAtividade({id:0});
     }
      setConfirmModal(!smShowConfirmModal);

    }
 
    
    const pegaTodasAtividades = async () => {
        const response = await api.get('atividade');
        return response.data;
    } 
    useEffect(() => {
    const getAtividades = async () => {
        const todasAtividades = await pegaTodasAtividades();
        if(todasAtividades) setAtividades(todasAtividades);

    }
    getAtividades();
  }, []);

  const addAtividade = async (ativ) => {
        const response = await api.post('atividade', ativ);
        console.log(response)
      setAtividades([...atividades, response.data]);
      handleStateModal();
  }

  const cancelarAtividade = () => {
      setAtividade({ id: 0 });
      handleStateModal();
  }

  const atualizarAtividade= async (ativ) => {
    const response = await api.put(`atividade/${ativ.id}`,ativ);
    const { id } = response.data;
    setAtividades(
          atividades.map((item) => (item.id === id ? response.data : item))
      );
      setAtividade({ id: 0 });
      handleStateModal();
  }

  const deletarAtividade= async (id) => {
    handleConfirmModal(0);
    if(await api.delete(`atividade/${id}`)){
        
    const atividadesFiltradas = atividades.filter(
        (atividade) => atividade.id !== id
    );
    setAtividades([...atividadesFiltradas]);
    }
    handleConfirmModal();
  }

  const pegarAtividade= (id) => {
      const atividade = atividades.filter((atividade) => atividade.id === id);
      setAtividade(atividade[0]);
      handleStateModal();
  }


  const novaAtividade =()=>{
    setAtividade({ id: 0 });
      handleStateModal();
  }
  return (
    <>
    <div className='d-flex justify-content-between align-items-end mt-2 pb-3 border-bottom border-dark'>
       <h1 className='m-0 p-0'>Atividade {atividade.id !== 0 ? atividade.id : ''}</h1> 

    <Button variant="outline-info" onClick={novaAtividade}>
        <i className='fas fa-plus'/>
      </Button>
    </div>


        <AtividadeLista
            atividades={atividades}
            pegarAtividade={pegarAtividade}
            handleConfirmModal={handleConfirmModal}

        />
      <Modal show={showAtividadeModal} onHide={handleStateModal}>
        <Modal.Header closeButton>
          <Modal.Title>Atividade {atividade.id !== 0 ? atividade.id : ''}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <AtividadeForm
            addAtividade={addAtividade}
            cancelarAtividade={cancelarAtividade}
            atualizarAtividade={atualizarAtividade}
            ativSelecionada={atividade}
            atividades={atividades}
        />
        </Modal.Body>
        
      </Modal>
      <Modal    size='sm'
                show={smShowConfirmModal}
                onHide={handleConfirmModal}>
        <Modal.Header closeButton>
          <Modal.Title>Excluindo Atividade{' '}
          {atividade.id !== 0 ? atividade.id : ''}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
            Tem certeza que deseja deletar esse registro? isso não é reversível? {atividade.id}
        </Modal.Body>
        <Modal.Footer>
        <button className='btn btn-outline-sucess me-2' onClick={()=>deletarAtividade(atividade.id)}>
                <i className='fas fa-check me-2'></i>
                Sim
            </button>
            <button className='btn btn-outline-danger me-2'
            onClick={()=> handleConfirmModal(0)}>
            <i className='fas fa-times me-2'></i>
                Não
            </button>
        </Modal.Footer>
        
      </Modal>
    </>
);
  }

export default App;
