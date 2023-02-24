import { Layout, Space, Row, Col, Card, Pagination } from 'antd';
import { useState } from 'react';

import { NavLink } from 'react-router-dom';


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

    return   (
    <Space
      direction="vertical"
      style={{
        width: '100%',
        height: '100%'
      }}
      size={[0, 48]}
    >
        <Content style={contentStyle}>
          <Row gutter={[16, 16]} justify={"space-between"}>
          {dogs.slice(Math.max(0, (page-1)*elementsPerPage), Math.min(page*elementsPerPage, dogs.length))
          .map(element => (
            <Col span={6}>
              <NavLink to={`/breeds/${element.id}`}>
                <Card
                  id={element.id}
                  hoverable
                  style={{ width: 240 }}
                  cover={<img alt="dog" src={element.image_url} />}>
    
                    <Meta title={element.name} description={element.breed_group}></Meta>
                </Card>
              </NavLink>
            </Col>
          ))}
          </Row>
        
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
    </Space>
    );
 }

export default DogsList;