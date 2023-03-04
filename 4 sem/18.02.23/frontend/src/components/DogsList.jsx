<<<<<<< HEAD
import { Space, Row, Col, Card, Pagination } from 'antd';
import { useEffect } from 'react';
=======
import { Layout, Space, Row, Col, Card, Pagination } from 'antd';
import { useState } from 'react';
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131

import { NavLink } from 'react-router-dom';


<<<<<<< HEAD
const { Meta } = Card;



const pageSizeOptions = [5, 10, 50, 100];

 const DogsList = ({dogs, totalDogs, itemsPerPage, page, setItemsPerPage, setPage}) =>
 {
    useEffect(() => { const href = window.location.href;
                      const indexOfId = href.indexOf("#");
                      const elem = href.substring(indexOfId+1);
                      document.getElementById(elem)?.scrollIntoView();
                    })
=======
const { Content } = Layout;
const { Meta } = Card;


const contentStyle = {
  textAlign: 'center',
  minHeight: 120,
  lineHeight: '120px',
  color: 'white',
  backgroundColor: '#108ee9',
};

const pageSizeOptions = [5, 10, 50, 100];

 const DogsList = ({dogs, totalDogs}) =>
 {
    const [page, setPage] = useState(1);
    const [elementsPerPage, setElementsPerPage] = useState(20);

>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
    return   (
    <Space
      direction="vertical"
      style={{
        width: '100%',
        height: '100%'
      }}
      size={[0, 48]}
    >
<<<<<<< HEAD
          <Row gutter={[16, 16]} justify={"space-between"}>
          {dogs.map(element => (
            <Col style={{padding: 20}}>
=======
        <Content style={contentStyle}>
          <Row gutter={[16, 16]} justify={"space-between"}>
          {dogs.slice(Math.max(0, (page-1)*elementsPerPage), Math.min(page*elementsPerPage, dogs.length))
          .map(element => (
            <Col span={6}>
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
              <NavLink to={`/breeds/${element.id}`}>
                <Card
                  id={element.id}
                  hoverable
<<<<<<< HEAD
                  style={{ width: 240, minHeight:300 }}
                  cover={<img alt="dog" style={{maxHeight: 200}}src={element.image_url} />}>
=======
                  style={{ width: 240 }}
                  cover={<img alt="dog" src={element.image_url} />}>
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
    
                    <Meta title={element.name} description={element.breed_group}></Meta>
                </Card>
              </NavLink>
            </Col>
          ))}
          </Row>
<<<<<<< HEAD
          
          <div>
          <Pagination
              style={{position:'absolute'}}
              total={totalDogs}
              showTotal={(totalDogs) => `Total ${totalDogs} breeds`}
              pageSize={itemsPerPage}
              current={page}
              onChange={(page, elementsOnPage) => {setPage(page); setItemsPerPage(elementsOnPage);}}
              onShowSizeChange={(size) => setItemsPerPage(size)}
              pageSizeOptions={pageSizeOptions}
          />
          <NavLink style={{position: "absolute", right: 10}} to="/review">Leave review about our service</NavLink>
          </div>

=======
        
          <Pagination
              total={dogs.length}
              showTotal={(total) => `Total ${total} breeds`}
              defaultPageSize={elementsPerPage}
              defaultCurrent={page}
              onChange={(page, elementsOnPage) => {setPage(page); setElementsPerPage(elementsOnPage);}}
              onShowSizeChange={(size) => setElementsPerPage(size)}
              pageSizeOptions={pageSizeOptions}
          />
        </Content>
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
    </Space>
    );
 }

export default DogsList;